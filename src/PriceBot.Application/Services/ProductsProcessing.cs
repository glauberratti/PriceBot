using PriceBot.Application.Interfaces;
using PriceBot.CrossCutting.Log;
using PriceBot.Domain.Product;
using PriceBot.Domain.Queue;
using PriceBot.Domain.SharedKernel.Enums;
using System.Net;

namespace PriceBot.Application.Services;

public class ProductsProcessing : IProductsProcessing
{
    private readonly IProductService _productService;
    private readonly ICurrencyService _currencyService;
    private readonly IProductsReprocessingQueue _productsReprocessingQueue;

    public ProductsProcessing(IProductService productService, ICurrencyService currencyService, IProductsReprocessingQueue productsReprocessingQueue)
    {
        _productService = productService;
        _currencyService = currencyService;
        _productsReprocessingQueue = productsReprocessingQueue;
    }

    public async Task ProcessUsdValues()
    {
        decimal usdValue = await _currencyService.GetCurrencyValue(Currency.USD);
        await UpdateProductsCurrency(Currency.USD, usdValue);
    }

    public async Task ProcessEurValues()
    {
        decimal eurValue = await _currencyService.GetCurrencyValue(Currency.EUR);
        await UpdateProductsCurrency(Currency.EUR, eurValue);
    }

    private async Task UpdateProductsCurrency(Currency currency, decimal currencyValue)
    {
        LoggerHelp.LogInfo($"Updating {currency.Value} values of all products.");
        IEnumerable<Product> products = await _productService.GetAllAsync();

        foreach (Product product in products)
        {
            product.UpdateCurrencyValue(currency, currencyValue);

            try
            {
                if (ThrowException())
                    throw new HttpListenerException();

                LoggerHelp.LogInfo($"Updating {currency.Value} value of product with id '{product.Id}'.");
                await _productService.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                LoggerHelp.LogError(ex, $"Error updating {currency.Value} value of product with id '{product.Id}'.");
                _productsReprocessingQueue.PublishMessage(product.Id);
            }
        }
    }

    public async Task ReprocessValueProduct(Currency currency)
    {
        Guid? productId;

        while (true)
        {
            try
            {
                productId = _productsReprocessingQueue.GetMessage(true);
            }
            catch (Exception)
            {
                return;
            }

            if (productId is null || productId.Equals(Guid.Empty))
                return;

            Product? product = await _productService.GetByIdAsync(productId.Value);
            if (product is null)
                return;

            try
            {
                LoggerHelp.LogInfo($"Updating {currency.Value} value of product with id '{product.Id}'.");
                decimal currencyValue;

                currencyValue = await _currencyService.GetCurrencyValue(currency);
                product.UpdateCurrencyValue(currency, currencyValue);
                
                await _productService.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                LoggerHelp.LogError(ex, $"Error updating {currency.Value} value of product with id '{product.Id}'.");
                _productsReprocessingQueue.PublishMessage(productId.Value);
                return;
            }
        }
    }

    private static bool ThrowException()
    {
        Random r = new(DateTime.Now.Second+DateTime.Now.Millisecond);
        int num = r.Next(0, 10);

        if (num == 0) return true;

        return false;
    }
}
