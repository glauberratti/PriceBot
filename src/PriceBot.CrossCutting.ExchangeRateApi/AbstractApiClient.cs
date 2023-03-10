using Microsoft.Extensions.Options;
using PriceBot.CrossCutting.CurrencyApi.Response;
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
            // TODO: Log
            HttpResponseMessage response = await _httpClient.GetAsync($"?{_settings.CurrencyApi.EndPointLatest}={_settings.CurrencyApi.Key}&base={currency.Value}");
            return response;
        }
        catch (Exception)
        {
            // TODO: Log
            throw;
        }
        finally
        {
            _httpClient.Dispose();
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

        // TODO: if not success

        return res.exchange_rates.BRL;
    }
}
