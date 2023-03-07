using PriceBot.Application.Interfaces;
using PriceBot.CrossCutting.ExchangeRateApi;
using PriceBot.Domain.SharedKernel.Enums;

namespace PriceBot.Application.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ExchangeRateApiClient _exchangeRateApiClient;

    public CurrencyService(ExchangeRateApiClient exchangeRateApiClient)
    {
        _exchangeRateApiClient = exchangeRateApiClient;
    }

    public async Task<decimal> GetUsdValue()
    {
        return await _exchangeRateApiClient.Get(Currency.USD);
    }

    public async Task<decimal> GetEurValue()
    {
        return await _exchangeRateApiClient.Get(Currency.EUR);
    }
}
