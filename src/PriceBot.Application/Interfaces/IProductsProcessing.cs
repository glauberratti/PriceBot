namespace PriceBot.Application.Interfaces;

public interface IProductsProcessing
{
    Task ProcessUsdValues();
    Task ProcessEurValues();
}
