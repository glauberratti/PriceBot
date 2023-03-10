using PriceBot.Domain.SharedKernel.Enums;

namespace PriceBot.CrossCutting.CurrencyApi;

public interface ICurrencyApiClient
{
    Task<decimal> Get(Currency currency);
}
