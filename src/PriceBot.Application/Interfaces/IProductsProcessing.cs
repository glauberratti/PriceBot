using PriceBot.Domain.SharedKernel.Enums;

namespace PriceBot.Application.Interfaces;

public interface IProductsProcessing
{
    Task ProcessUsdValues();
    Task ProcessEurValues();
    Task ReprocessValueProduct(Currency currency);
}
