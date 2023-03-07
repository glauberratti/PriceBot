using PriceBot.Domain.SharedKernel.Enums;

namespace PriceBot.CrossCutting.ExchangeRateApi;

public interface IExchangeRateApiClient
{
    Task<decimal> Get(Currency currency);
}
