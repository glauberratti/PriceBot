using PriceBot.Application.Interfaces;
using PriceBot.Domain.Product;
using PriceBot.Domain.Queue;
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
        decimal usdValue = await _currencyService.GetUsdValue();
        var (ok, errors) = await UpdateProductsUsdCurrency(usdValue);
    }

    public async Task ProcessEurValues()
    {
        decimal eurValue = await _currencyService.GetEurValue();
        var (ok, errors) = await UpdateProductsEurCurrency(eurValue);
    }

    private async Task<(bool, List<string>)> UpdateProductsUsdCurrency(decimal usdValue)
    {
        List<string> erros = new();

        IEnumerable<Product> products = await _productService.GetAllAsync();

        foreach (Product product in products)
        {
            product.UpdateUsdCurrency(usdValue);

            try
            {
                if (ThrowException())
                    throw new HttpListenerException();

                // TODO: Log
                await _productService.UpdateAsync(product);
            }
            catch (Exception)
            {
                // TODO: Log
                _productsReprocessingQueue.PublishMessage(product.Id);
            }
        }

        return (true, erros);
    }

    private async Task<(bool, List<string>)> UpdateProductsEurCurrency(decimal eurValue)
    {
        List<string> erros = new();

        IEnumerable<Product> products = await _productService.GetAllAsync();

        foreach (Product product in products)
        {
            product.UpdateEurCurrency(eurValue);

            try
            {
                if (ThrowException())
                    throw new HttpListenerException();

                // TODO: Log
                await _productService.UpdateAsync(product);
            }
            catch (Exception)
            {
                // TODO: Log
                _productsReprocessingQueue.PublishMessage(product.Id);
            }
        }

        return (true, erros);
    }

    public async Task<bool> ReprocessUsdValueProduct()
    {
        Guid? productId = _productsReprocessingQueue.GetMessage(true);
        if (productId is null || productId.Equals(Guid.Empty))
            return true;

        Product? product = await _productService.GetByIdAsync(productId.Value);
        if (product is null)
            return true;

        try
        {
            // TODO: Log
            decimal usdValue = await _currencyService.GetUsdValue();
            product.UpdateUsdCurrency(usdValue);
            await _productService.UpdateAsync(product);
        }
        catch (Exception)
        {
            // TODO: Log
            _productsReprocessingQueue.PublishMessage(productId.Value);
            return false;
        }

        return true;
    }

    private bool ThrowException()
    {
        Random r = new(DateTime.Now.Second+DateTime.Now.Millisecond);
        int num = r.Next(0, 10);

        if (num == 0) return true;

        return false;
    }
}
