using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoParserServer.JsonClasses.Cryptorank;
using CryptoParserServer.JsonClasses.Internal;

namespace CryptoParserServer
{
    public class СryptorankApi
    {
        private static RestClient _client = new RestClient("https://api.cryptorank.io/");

        public static int ThreadsCount = 15;

        public static T GetURLData<T>(string URL)
        {
            RestRequest request = new RestRequest(URL, Method.Get);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");

            RestResponse response = _client.Execute(request);
            Encoding encoding = Encoding.GetEncoding("utf-8");

            return JsonConvert.DeserializeObject<T>(encoding.GetString(response.RawBytes));
        }

        public static string GetURLData(string URL)
        {
            RestRequest request = new RestRequest(URL, Method.Get);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");

            RestResponse response = _client.Execute(request);
            Encoding encoding = Encoding.GetEncoding("utf-8");

            return encoding.GetString(response.RawBytes);
        }

        public static List<ListDatum> GetGainersCurrencies()
        {
            string url = "/v0/coins?specialFilter=topGainersFor24h&limit=50";
            ListRoot response = GetURLData<ListRoot>(url);

            return response.Data;
        }

        public static List<ListDatum> GetLosersCurrencies()
        {
            string url = "/v0/coins?specialFilter=topLosersFor24h&limit=50";
            ListRoot response = GetURLData<ListRoot>(url);

            return response.Data;
        }

        public static List<Arbitrage> GetArbitrageForCurrency(ListDatum currency)
        {
            try
            {
                string url = $"https://cryptorank.io/ru/price/{currency.Key}/arbitrage";
                string Data = GetURLData(url);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(Data);

                var elements = doc.DocumentNode.Descendants("tr").ToList();
                List<Market> sell = new List<Market>(), buy = new List<Market>();


                foreach (var element in elements[0].ChildNodes.Skip(1))
                    sell.Add(new Market(element.FirstChild.InnerText,
                        Convert.ToDouble(element.ChildNodes[2].InnerText.Replace("$ ", ""), System.Globalization.CultureInfo.InvariantCulture),
                        element.ChildNodes[4].InnerText));

                foreach (var _element in elements.Skip(1))
                {
                    var element = _element.FirstChild;
                    if (element.ChildNodes.Count == 5)
                        buy.Add(new Market(element.FirstChild.InnerText,
                        Convert.ToDouble(element.ChildNodes[2].InnerText.Replace("$ ", ""), System.Globalization.CultureInfo.InvariantCulture),
                        element.ChildNodes[4].InnerText));
                    else
                        buy.Add(new Market(element.FirstChild.InnerText,
                        Convert.ToDouble(element.ChildNodes[3].InnerText.Replace("$ ", ""), System.Globalization.CultureInfo.InvariantCulture),
                        element.ChildNodes[5].InnerText));
                }


                List<Arbitrage> arbitrages = new List<Arbitrage>();
                for (int i = 0; i < buy.Count; i++)
                {
                    var html_raw = elements[i + 1];

                    for (int j = 0; j < sell.Count; j++)
                    {
                        if (html_raw.ChildNodes[j + 1].InnerText == "") continue;

                        double percent = Convert.ToDouble(html_raw.ChildNodes[j + 1].InnerText.Replace("%", ""), System.Globalization.CultureInfo.InvariantCulture);
                        if (percent <= 0) continue;

                        arbitrages.Add(new Arbitrage() { MarketFrom = buy[i], MarketTo = sell[j], Percent = percent, Currency = currency.Name, CurrencyKey = currency.Key });
                    }
                }

                return arbitrages;
            }
            catch
            {
                return new List<Arbitrage>();
            }
        }

        private static List<Arbitrage> ThreadFunc(object arg)
        {
            List<ListDatum> listData = (List<ListDatum>)arg;
            List<Arbitrage> ans = new List<Arbitrage>();
            foreach (ListDatum element in listData)
            {
                var getArbitrages = GetArbitrageForCurrency(element);
                foreach (Arbitrage arbitrage in getArbitrages) ans.Add(arbitrage);
            }

            return ans;
        }

        public List<Arbitrage> GetBestArbitrages()
        {
            List<ListDatum> losers = GetLosersCurrencies(), gainers = GetGainersCurrencies();

            List<Arbitrage> arbitrages = new List<Arbitrage>();
            var tasks = new List<Task<List<Arbitrage>>>();

            for (int i = 0; i < ThreadsCount; i++)
            {
                tasks.Add(Task<List<Arbitrage>>.Factory.StartNew(ThreadFunc, losers.Skip(150 / ThreadsCount * i).Take(150 / ThreadsCount).ToList()));
            }

            for (int i = 0; i < ThreadsCount; i++)
            {
                tasks.Add(Task<List<Arbitrage>>.Factory.StartNew(ThreadFunc, gainers.Skip(150 / ThreadsCount * i).Take(150 / ThreadsCount).ToList()));
            }

            bool isOk = false;
            while (!isOk)
            {
                isOk = true;
                for (int i = 0; i < tasks.Count; i++) isOk = isOk && tasks[i].Result != null;
            }


            for (int i = 0; i < tasks.Count; i++)
            {
                for (int j = 0; j < tasks[i].Result.Count; j++)
                    arbitrages.Add(tasks[i].Result[j]);
            }

            arbitrages.Sort();
            arbitrages.Reverse();

            return arbitrages;
        }
    }
}
