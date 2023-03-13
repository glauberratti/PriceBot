using System.Text.Json.Serialization;

namespace PriceBot.CrossCutting.CurrencyApi.Response;

public class AbstractApiResponse
{
    [JsonPropertyName("base")]
    public string Base { get; set; } = string.Empty;

    [JsonPropertyName("last_updated")]
    public int Last_updated { get; set; }

    [JsonPropertyName("exchange_rates")]
    public ExchangeRates Exchange_rates { get; set; } = new();
}

public class ExchangeRates
{
    public decimal BRL { get; set; }
}
