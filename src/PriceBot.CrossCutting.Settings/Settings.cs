namespace PriceBot.CrossCutting.Settings
{
    public class Settings
    {
        public ExchangeRateApi ExchangeRateApi { get; set; } = new();
    }

    public class ExchangeRateApi
    {
        public string Url { get; set; } = string.Empty;
        public string EndPointLatest { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }
}

