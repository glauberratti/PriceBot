namespace PriceBot.CrossCutting.Settings
{
    public class Settings
    {
        public CurrencyApi CurrencyApi { get; set; } = new();
        public RabbitMQConfig RabbitMQConfig { get; set; } = new();
    }

    public class CurrencyApi
    {
        public string Url { get; set; } = string.Empty;
        public string EndPointLatest { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }

    public class RabbitMQConfig
    {
        public string VirtualHost { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ProductsReprocessingQueue { get; set; } = string.Empty;
    }
}

