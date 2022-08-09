using Newtonsoft.Json;
using System.Collections.Generic;

namespace CryptoParserServer.JsonClasses.Cryptorank
{
    public class _1Y
    {
        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }
    }

    public class _24H
    {
        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }
    }

    public class _30D
    {
        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }
    }

    public class _3M
    {
        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }
    }

    public class _6M
    {
        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }
    }

    public class _7D
    {
        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }
    }

    public class AthPrice
    {
        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }

        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("dateBTC")]
        public string DateBTC { get; set; }

        [JsonProperty("dateETH")]
        public string DateETH { get; set; }
    }

    public class AtlPrice
    {
        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }

        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("dateBTC")]
        public string DateBTC { get; set; }

        [JsonProperty("dateETH")]
        public string DateETH { get; set; }

        [JsonProperty("dateUSD")]
        public string DateUSD { get; set; }
    }

    public class ListDatum
    {
        [JsonProperty("rank")]
        public long? Rank { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("marketCap")]
        public double? MarketCap { get; set; }

        [JsonProperty("availableSupply")]
        public object AvailableSupply { get; set; }

        [JsonProperty("fullyDilutedMarketCap")]
        public double? FullyDilutedMarketCap { get; set; }

        [JsonProperty("maxSupply")]
        public object MaxSupply { get; set; }

        [JsonProperty("unlimitedSupply")]
        public bool UnlimitedSupply { get; set; }

        [JsonProperty("totalSupply")]
        public object TotalSupply { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("tokens")]
        public List<Token> Tokens { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("categoryId")]
        public long CategoryId { get; set; }

        [JsonProperty("tagIds")]
        public List<long> TagIds { get; set; }

        [JsonProperty("fundIds")]
        public List<long> FundIds { get; set; }

        [JsonProperty("longerest")]
        public object longerest { get; set; }

        [JsonProperty("isTraded")]
        public bool IsTraded { get; set; }

        [JsonProperty("volume24h")]
        public double Volume24h { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("histPrices")]
        public HistPrices HistPrices { get; set; }

        [JsonProperty("athPrice")]
        public AthPrice AthPrice { get; set; }

        [JsonProperty("atlPrice")]
        public AtlPrice AtlPrice { get; set; }

        [JsonProperty("volatility")]
        public Volatility Volatility { get; set; }

        [JsonProperty("icoFullyDilutedMarketCap")]
        public double? IcoFullyDilutedMarketCap { get; set; }

        [JsonProperty("icoStatus")]
        public string IcoStatus { get; set; }

        [JsonProperty("initialSupply")]
        public long? InitialSupply { get; set; }
    }

    public class HistPrices
    {
        [JsonProperty("7D")]
        public _7D _7D { get; set; }

        [JsonProperty("6M")]
        public _6M _6M { get; set; }

        [JsonProperty("1Y")]
        public _1Y _1Y { get; set; }

        [JsonProperty("YTD")]
        public YTD YTD { get; set; }

        [JsonProperty("3M")]
        public _3M _3M { get; set; }

        [JsonProperty("24H")]
        public _24H _24H { get; set; }

        [JsonProperty("30D")]
        public _30D _30D { get; set; }
    }

    public class Image
    {
        [JsonProperty("native")]
        public string Native { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("x60")]
        public string X60 { get; set; }

        [JsonProperty("x150")]
        public string X150 { get; set; }
    }

    public class Price
    {
        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }
    }

    public class ListRoot
    {
        [JsonProperty("data")]
        public List<ListDatum> Data { get; set; }
    }

    public class Token
    {
        [JsonProperty("platformName")]
        public string PlatformName { get; set; }

        [JsonProperty("platformKey")]
        public string PlatformKey { get; set; }

        [JsonProperty("platformSlug")]
        public string PlatformSlug { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }

    public class Volatility
    {
        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }
    }

    public class YTD
    {
        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("BTC")]
        public double BTC { get; set; }

        [JsonProperty("ETH")]
        public double ETH { get; set; }
    }
}
