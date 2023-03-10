namespace PriceBot.CrossCutting.CurrencyApi.Response;

public class AbstractApiResponse
{
    public string @base { get; set; } = string.Empty;
    public int last_updated { get; set; }
    public ExchangeRates exchange_rates { get; set; } = new();
}

public class ExchangeRates
{
    public decimal BRL { get; set; }
}
