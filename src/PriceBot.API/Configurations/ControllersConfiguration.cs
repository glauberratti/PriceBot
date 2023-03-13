using PriceBot.Application.Interfaces;
using PriceBot.Application.ViewModels;
using PriceBot.Application.Mappings;
using PriceBot.Domain.SharedKernel.Enums;

namespace PriceBot.API.Configurations;

public static class ControllersConfiguration
{
    public static WebApplication UseControllersConfiguration(this WebApplication app)
    {
        app.MapGet("/get-product", async (Guid id, IProductService productService) =>
        {
            var product = await productService.GetByIdAsync(id);

            if (product is null)
                return Results.NotFound();

            return Results.Ok(product.Map());
        })
        .WithOpenApi();

        app.MapGet("/get-random-product", async (IProductService productService) =>
        {
            var product = await productService.GetRandomAsync();

            return Results.Ok(product);
        })
        .WithOpenApi();

        app.MapPost("/post-product", async (ProductRequestVM productVM, IProductService productService) =>
        {
            try
            {
                await productService.AddAsync(productVM.Map());
                return Results.Ok(productVM);
            }
            catch (Exception)
            {
                return Results.StatusCode(500);
            }
        })
        .WithOpenApi();

        app.MapGet("/process-usd-values", async (IProductsProcessing productsProcessing) =>
        {
            await productsProcessing.ProcessUsdValues();
        });

        app.MapGet("/process-eur-values", async (IProductsProcessing productsProcessing) =>
        {
            await productsProcessing.ProcessEurValues();
        });

        app.MapGet("/reprocess-usd-value-product", async (IProductsProcessing productsProcessing) =>
        {
            await productsProcessing.ReprocessValueProduct(Currency.USD);
        });

        return app;
    }
}
