using PriceBot.Application.Interfaces;
using PriceBot.CrossCutting.Log;
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
        // TODO: rethink about (ok, errors)
        await UpdateProductsUsdCurrency(usdValue);
    }

    public async Task ProcessEurValues()
    {
        decimal eurValue = await _currencyService.GetEurValue();
        // TODO: rethink about (ok, errors)
        await UpdateProductsEurCurrency(eurValue);
    }

    private async Task UpdateProductsUsdCurrency(decimal usdValue)
    {
        // TODO: Refactor to one method
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
    }

    private async Task UpdateProductsEurCurrency(decimal eurValue)
    {
        // TODO: Refactor to one method
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
    }

    public async Task ReprocessUsdValueProduct()
    {
        // TODO: Refactor to one method
        Guid? productId;
        try
        {
            productId = _productsReprocessingQueue.GetMessage(true);
        }
        catch (Exception ex)
        {
            // TODO: Log
            LoggerHelp.LogError(ex, "Error trying to get USD value from currency API");
            return;
        }

        if (productId is null || productId.Equals(Guid.Empty))
            return;

        Product? product = await _productService.GetByIdAsync(productId.Value);
        if (product is null)
            return;

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
            return;
        }

        return;
    }

    public async Task ReprocessEurValueProduct()
    {
        // TODO: Refactor to one method
        Guid? productId;
        try
        {
            productId = _productsReprocessingQueue.GetMessage(true);
        }
        catch (Exception ex)
        {
            LoggerHelp.LogError(ex, "Error trying to get EUR value from currency API");
            return;
        }

        if (productId is null || productId.Equals(Guid.Empty))
            return;

        Product? product = await _productService.GetByIdAsync(productId.Value);
        if (product is null)
            return;

        try
        {
            // TODO: Log
            decimal eurValue = await _currencyService.GetEurValue();
            product.UpdateUsdCurrency(eurValue);
            await _productService.UpdateAsync(product);
        }
        catch (Exception)
        {
            // TODO: Log
            _productsReprocessingQueue.PublishMessage(productId.Value);
            return;
        }

        return;
    }

    private static bool ThrowException()
    {
        Random r = new(DateTime.Now.Second+DateTime.Now.Millisecond);
        int num = r.Next(0, 10);

        if (num == 0) return true;

        return false;
    }
}
