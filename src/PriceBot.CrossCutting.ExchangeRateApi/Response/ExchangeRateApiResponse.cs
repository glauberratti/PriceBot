namespace PriceBot.CrossCutting.ExchangeRateApi.Response;

public class ExchangeRateApiResponse
{
    public string result { get; set; } = string.Empty;
    public string documentation { get; set; } = string.Empty;
    public string terms_of_use { get; set; } = string.Empty;
    public int time_last_update_unix { get; set; }
    public string time_last_update_utc { get; set; } = string.Empty;
    public int time_next_update_unix { get; set; }
    public string time_next_update_utc { get; set; } = string.Empty;
    public string base_code { get; set; } = string.Empty;
    public ConversionRates conversion_rates { get; set; } = new();
}

public class ConversionRates
{
    public decimal BRL { get; set; }
}
