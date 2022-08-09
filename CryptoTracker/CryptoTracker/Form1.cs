using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoTracker
{
    public partial class Form1 : Form
    {
        public static bool isEnabled = false;

        public static RestClient client;

        public static List<CryptoParserServer.JsonClasses.Internal.Arbitrage> Arbitrages = new List<CryptoParserServer.JsonClasses.Internal.Arbitrage>();

        public static string hwid = HWID.GetHardwareId();

        public static List<string> fiats = new List<string>();

        public Form1()
        {
            InitializeComponent();
            this.MinimumSize = this.MaximumSize = this.Size;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void авторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Irval1337");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            isEnabled = false;
            button1.Text = "Начать парсинг";

            char number = e.KeyChar;

            if (number == 8) return;

            if (!Char.IsDigit(number) || Convert.ToInt32(textBox1.Text + number) > 100)
            {
                e.Handled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            App.LoadSettings();
            client = new RestClient(App.Server);
            textBox1.Text = App.Settings.MinPercent.ToString();
            textBox2.Text = App.Settings.MaxPercent.ToString();
            checkBox1.Checked = App.Settings.DrawCurrencies;
            checkBox2.Checked = App.Settings.DrawFiats;

            textBox3.Text = "*";
            textBox4.Text = "*";

            label2.Text += App.Version;

            RestRequest request = new RestRequest("CurrentVersion", Method.Get);
            Encoding encoding = Encoding.GetEncoding("utf-8");
            RestResponse response = client.Execute(request);
            string server_version = encoding.GetString(response.RawBytes);
            if (server_version != App.Version)
            {
                MessageBox.Show("Ваша версия программы устарела! Последняя актуальная: " + server_version);
                Process.Start("Updater.exe");
                Environment.Exit(0);
            }

            request = new RestRequest("Auth", Method.Get);
            request.AddParameter("hwid", hwid);
            response = client.Execute(request);
            
            var user = JsonConvert.DeserializeObject<CryptoParserServer.JsonClasses.Internal.User>(encoding.GetString(response.RawBytes));

            if (user == null || user.Status == CryptoParserServer.JsonClasses.Internal.User.UserStatus.Registered)
            {
                MessageBox.Show("У вас отсутствует лицензия на данный продукт. Обратитесь к @JingoBellsJA (Telegram) для ее приобретения.\nВаш HWID скопирован в буфер обмена.", "CryptoTracker");
                Clipboard.SetText(hwid);
                Environment.Exit(0);
            }
            else
            {
                this.Text += $" [Авторизирован {user.Username} до {user.LicenseExpiration.AddHours(3).ToString()}]";
            }

            {
                RestClient restClient = new RestClient("https://raw.githubusercontent.com/");
                request = new RestRequest("mhs/world-currencies/master/currencies.json", Method.Get);
                response = restClient.Execute(request);
                var fullFiats = JsonConvert.DeserializeObject<List<CryptoParserServer.JsonClasses.Internal.Fiat>>(encoding.GetString(response.RawBytes));
                fullFiats.ForEach(x => fiats.Add(x.cc));
            }
        }

        void listbox_update()
        {
            Task.Factory.StartNew(() =>
            {
                RestRequest request = new RestRequest("GetBestArbitrages", Method.Get);

                request.AddParameter("hwid", hwid);

                RestResponse response = client.Execute(request);
                Encoding encoding = Encoding.GetEncoding("utf-8");

                var arbitrages = JsonConvert.DeserializeObject<List<CryptoParserServer.JsonClasses.Internal.Arbitrage>>(encoding.GetString(response.RawBytes));

                this.BeginInvoke((Action)(() =>
                {
                    this.listBox1.Items.Clear();
                }));

                if (!isEnabled) return;

                Arbitrages.Clear();

                bool needFilterFrom = true, needFilterTo = true;

                if (string.IsNullOrEmpty(textBox3.Text) || textBox3.Text == "*") needFilterFrom = false;
                if (string.IsNullOrEmpty(textBox4.Text) || textBox4.Text == "*") needFilterTo = false;

                foreach (var arbitrage in arbitrages)
                {
                    if (App.Settings.BlackListedMarkets.Contains(arbitrage.MarketFrom.Name) || App.Settings.BlackListedMarkets.Contains(arbitrage.MarketTo.Name)) continue;

                    if (App.Settings.BlackListedCoins.Contains(arbitrage.MarketFrom.Currency.Split('/')[0])) continue;
                    if (App.Settings.BlackListedCoins.Contains(arbitrage.MarketFrom.Currency.Split('/')[1])) continue;

                    if (App.Settings.BlackListedCoins.Contains(arbitrage.MarketTo.Currency.Split('/')[0])) continue;
                    if (App.Settings.BlackListedCoins.Contains(arbitrage.MarketTo.Currency.Split('/')[1])) continue;

                    if (arbitrage.Percent > (string.IsNullOrEmpty(textBox2.Text) ? 0 : Convert.ToInt32(textBox2.Text))) continue;
                    if (arbitrage.Percent < (string.IsNullOrEmpty(textBox1.Text) ? 0 : Convert.ToInt32(textBox1.Text))) break;

                    if ((needFilterFrom && arbitrage.MarketFrom.Currency.Split('/')[1] != textBox3.Text) || (needFilterTo && arbitrage.MarketTo.Currency.Split('/')[1] != textBox4.Text)) continue;

                    if (!App.Settings.DrawFiats && !fiats.Contains(textBox3.Text) && !fiats.Contains(textBox4.Text))
                    {
                        if (fiats.Contains(arbitrage.MarketFrom.Currency.Split('/')[0])) continue;
                        if (fiats.Contains(arbitrage.MarketFrom.Currency.Split('/')[1])) continue;

                        if (fiats.Contains(arbitrage.MarketTo.Currency.Split('/')[0])) continue;
                        if (fiats.Contains(arbitrage.MarketTo.Currency.Split('/')[1])) continue;
                    }

                    Arbitrages.Add(arbitrage);

                    this.BeginInvoke((Action)(() =>
                    {
                        if (checkBox1.Checked)
                            this.listBox1.Items.Add($"{arbitrage.Percent}% {arbitrage.MarketFrom.Name} ({arbitrage.MarketFrom.Currency}) -> {arbitrage.MarketTo.Name} ({arbitrage.MarketTo.Currency})");
                        else
                            this.listBox1.Items.Add($"{arbitrage.Percent}% {arbitrage.MarketFrom.Name} -> {arbitrage.MarketTo.Name}");
                    }));
                }
            }).Wait();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isEnabled = !isEnabled;

            if (isEnabled) button1.Text = "Остановить парсинг";
            else button1.Text = "Начать парсинг";

            if (isEnabled) {
                Task.Factory.StartNew(() => {
                    while (isEnabled)
                    {
                        listbox_update();

                        Thread.Sleep(10000);
                    }
                });
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index == -1 || index >= Arbitrages.Count) return;

            var arbitrage = Arbitrages[index];

            MarketBuyName.Text = "Биржа покупки: " + arbitrage.MarketFrom.Name;
            MarketBuyCurrency.Text = "Валюта покупки: " + arbitrage.MarketFrom.Currency;
            MarketBuyPrice.Text = "Стоимость покупки: " + arbitrage.MarketFrom.Price.ToString() + " $";

            MarketSellName.Text = "Биржа продажи: " + arbitrage.MarketTo.Name;
            MarketSellCurrency.Text = "Валюта продажи: " + arbitrage.MarketTo.Currency;
            MarketSellPrice.Text = "Стоимость продажи: " + arbitrage.MarketTo.Price.ToString() + " $";

            Percent.Text = "Процент прибыли: " + arbitrage.Percent.ToString() + "%";

            OpenLink.Tag = $"https://cryptorank.io/ru/price/{arbitrage.CurrencyKey}/arbitrage";
        }

        private void OpenLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (OpenLink.Tag == null || string.IsNullOrEmpty((string)OpenLink.Tag)) return;

            Process.Start((string)OpenLink.Tag);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            App.Settings.MinPercent = string.IsNullOrEmpty(textBox1.Text) ? 0 : Convert.ToInt32(textBox1.Text);
            App.Settings.MaxPercent = string.IsNullOrEmpty(textBox2.Text) ? 0 : Convert.ToInt32(textBox2.Text);
            App.SaveSettings();
            Environment.Exit(0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (App.Settings.DrawCurrencies != checkBox1.Checked)
            {
                App.Settings.DrawCurrencies = checkBox1.Checked;
                listbox_update();
            }
        }

        private void биржиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isEnabled = false;
            button1.Text = "Начать парсинг";

            new BlackListMarkets().ShowDialog();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            isEnabled = false;
            button1.Text = "Начать парсинг";

            char number = e.KeyChar;

            if (number == 8) return;

            if (!Char.IsDigit(number) || Convert.ToInt32(textBox2.Text + number) > 100)
            {
                e.Handled = true;
            }
        }

        private void валютаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isEnabled = false;
            button1.Text = "Начать парсинг";

            new BlackListCoins().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listbox_update();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.DrawFiats = checkBox2.Checked;
            listbox_update();
        }
    }
}
