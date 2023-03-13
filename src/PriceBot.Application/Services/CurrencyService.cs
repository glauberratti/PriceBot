using PriceBot.Application.Interfaces;
using PriceBot.CrossCutting.CurrencyApi;
using PriceBot.Domain.SharedKernel.Enums;

namespace PriceBot.Application.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ICurrencyApiClient _currencyApiClient;

    public CurrencyService(ICurrencyApiClient currencyApiClient)
    {
        _currencyApiClient = currencyApiClient;
    }

    public async Task<decimal> GetCurrencyValue(Currency currency)
    {
        return await _currencyApiClient.Get(currency);
    }
}
