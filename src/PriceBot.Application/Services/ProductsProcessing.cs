using PriceBot.Application.Interfaces;
using PriceBot.Domain.Product;
using System.Net;

namespace PriceBot.Application.Services;

public class ProductsProcessing : IProductsProcessing
{
    private readonly IProductService _productService;
    private readonly ICurrencyService _currencyService;

    public ProductsProcessing(IProductService productService, ICurrencyService currencyService)
    {
        _productService = productService;
        _currencyService = currencyService;
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

                await _productService.UpdateAsync(product);
            }
            catch (Exception)
            {
                // TODO: Add on reprocessing queue
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

                await _productService.UpdateAsync(product);
            }
            catch (Exception)
            {
                // TODO: Add on reprocessing queue
            }
        }

        return (true, erros);
    }

    private bool ThrowException()
    {
        Random r = new(DateTime.Now.Second+DateTime.Now.Millisecond);
        int num = r.Next(0, 10);

        if (num == 0) return true;

        return false;
    }
}
