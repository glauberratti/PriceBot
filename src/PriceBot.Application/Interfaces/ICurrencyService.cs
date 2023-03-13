using PriceBot.Domain.SharedKernel.Enums;

namespace PriceBot.Application.Interfaces;

public interface ICurrencyService
{
    Task<decimal> GetCurrencyValue(Currency currency);
}
