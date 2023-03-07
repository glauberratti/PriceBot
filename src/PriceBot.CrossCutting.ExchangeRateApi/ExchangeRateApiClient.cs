using Microsoft.Extensions.Options;
using PriceBot.CrossCutting.ExchangeRateApi.Response;
using PriceBot.Domain.SharedKernel.Enums;
using System.Text.Json;

namespace PriceBot.CrossCutting.ExchangeRateApi;

public class ExchangeRateApiClient : IExchangeRateApiClient
{
    private readonly HttpClient _httpClient;
    private readonly Settings.Settings _settings;

    public ExchangeRateApiClient(HttpClient httpClient, IOptions<Settings.Settings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    private async Task<HttpResponseMessage> GetFromApi(Currency currency)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_settings.ExchangeRateApi.Key}/{_settings.ExchangeRateApi.EndPointLatest}/{currency.Value}");
        _httpClient.Dispose();
        return response;
    }

    public async Task<decimal> Get(Currency currency)
    {
        ExchangeRateApiResponse res = new();
        HttpResponseMessage response = await GetFromApi(currency);

        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            res = JsonSerializer.Deserialize<ExchangeRateApiResponse>(data) ?? new();
        }

        return res.conversion_rates.BRL;
    }
}
