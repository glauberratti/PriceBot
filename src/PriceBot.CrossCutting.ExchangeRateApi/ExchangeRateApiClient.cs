﻿using Microsoft.Extensions.Options;
using PriceBot.CrossCutting.CurrencyApi.Response;
using PriceBot.Domain.SharedKernel.Enums;
using System.Text.Json;

namespace PriceBot.CrossCutting.CurrencyApi;

public class ExchangeRateApiClient : ICurrencyApiClient
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
        try
        {
            // TODO: Log
            HttpResponseMessage response = await _httpClient.GetAsync($"{_settings.CurrencyApi.Key}/{_settings.CurrencyApi.EndPointLatest}/{currency.Value}");
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
        ExchangeRateApiResponse res = new();
        HttpResponseMessage response = await GetFromApi(currency);

        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            res = JsonSerializer.Deserialize<ExchangeRateApiResponse>(data) ?? new();
        }

        // TODO: if not success

        return res.conversion_rates.BRL;
    }
}