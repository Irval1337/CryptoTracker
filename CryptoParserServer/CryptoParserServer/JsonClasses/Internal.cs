using System;

namespace CryptoParserServer.JsonClasses.Internal
{
    public class Market
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Currency { get; set; }

        public Market(string name, double price, string currency)
        {
            Name = name;
            Price = price;
            Currency = currency;
        }
    }

    public class Arbitrage : IComparable
    {
        public double Percent { get; set; }

        public Market MarketFrom { get; set; }

        public Market MarketTo { get; set; }

        public string Currency { get; set; }

        public string CurrencyKey { get; set; }

        public int CompareTo(object o)
        {
            if (o is Arbitrage arbitrage) return Percent.CompareTo(arbitrage.Percent);
            else throw new ArgumentException("Некорректное значение параметра");
        }
    }
}
