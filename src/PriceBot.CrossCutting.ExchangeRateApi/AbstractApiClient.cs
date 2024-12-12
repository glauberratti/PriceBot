using Microsoft.Extensions.Options;
using PriceBot.CrossCutting.CurrencyApi.Response;
using PriceBot.CrossCutting.Log;
using PriceBot.Domain.SharedKernel.Enums;
using System.Text.Json;

namespace PriceBot.CrossCutting.CurrencyApi;

public class AbstractApiClient : ICurrencyApiClient
{
    private readonly HttpClient _httpClient;
    private readonly Settings.Settings _settings;

    public AbstractApiClient(HttpClient httpClient, IOptions<Settings.Settings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    private async Task<HttpResponseMessage> GetFromApi(Currency currency)
    {
        try
        {
            LoggerHelp.LogInfo($"Making a request to the currency API, to get {currency.Value} value.");
            HttpResponseMessage response = await _httpClient.GetAsync($"?{_settings.CurrencyApi.EndPointLatest}={_settings.CurrencyApi.Key}&base={currency.Value}");
            return response;
        }
        catch (Exception ex)
        {
            LoggerHelp.LogError(ex, "A unexpected error occurred while trying to request to the currency API");
            throw;
        }
    }

    public async Task<decimal> Get(Currency currency)
    {
        AbstractApiResponse res = new();
        HttpResponseMessage response = await GetFromApi(currency);

        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            res = JsonSerializer.Deserialize<AbstractApiResponse>(data) ?? new();
        }
        else
            LoggerHelp.LogError($"Unsuccessful response from currency API, returning code {response.StatusCode}");

        return res.Exchange_rates.BRL;
    }
}
