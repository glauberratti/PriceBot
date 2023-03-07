using PriceBot.CrossCutting.Response;
using System.Text.Json;

namespace PriceBot.CrossCutting;

public class ExchangeRateApiClient
{
    private readonly HttpClient _httpClient;

    public ExchangeRateApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task<HttpResponseMessage> GetFromApi()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("");
        return response;
    }

    public async Task<ExchangeRateApiResponse> Get()
    {
        ExchangeRateApiResponse res = new();
        HttpResponseMessage response = await GetFromApi();

        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            res = JsonSerializer.Deserialize<ExchangeRateApiResponse>(data) ?? new();
            return res;
        }

        return res;
    }
}
