using System.Text.Json.Serialization;

namespace PriceBot.CrossCutting.CurrencyApi.Response;

public class ExchangeRateApiResponse
{
    [JsonPropertyName("result")]
    public string Result { get; set; } = string.Empty;

    [JsonPropertyName("documentation")]
    public string Documentation { get; set; } = string.Empty;

    [JsonPropertyName("terms_of_use")]
    public string Terms_of_use { get; set; } = string.Empty;

    [JsonPropertyName("time_last_update_unix")]
    public int Time_last_update_unix { get; set; }

    [JsonPropertyName("time_last_update_utc")]
    public string Time_last_update_utc { get; set; } = string.Empty;

    [JsonPropertyName("time_next_update_unix")]
    public int Time_next_update_unix { get; set; }

    [JsonPropertyName("time_next_update_utc")]
    public string Time_next_update_utc { get; set; } = string.Empty;

    [JsonPropertyName("base_code")]
    public string Base_code { get; set; } = string.Empty;

    [JsonPropertyName("conversion_rates")]
    public ConversionRates Conversion_rates { get; set; } = new();
}

public class ConversionRates
{
    public decimal BRL { get; set; }
}
