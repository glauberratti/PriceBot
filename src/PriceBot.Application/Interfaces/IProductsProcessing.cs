namespace PriceBot.Application.Interfaces;

public interface IProductsProcessing
{
    Task ProcessUsdValues();
    Task ProcessEurValues();
    Task ReprocessUsdValueProduct();
    Task ReprocessEurValueProduct();
}
