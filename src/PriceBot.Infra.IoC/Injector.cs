using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceBot.Application.Interfaces;
using PriceBot.Application.Services;
using PriceBot.CrossCutting.CurrencyApi;
using PriceBot.Domain.Product.Repository;
using PriceBot.Domain.Queue;
using PriceBot.Infra.Data.Context;
using PriceBot.Infra.Data.Queue;

namespace PriceBot.Infra.IoC;

public static class Injector
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DbPriceBotContext>(options => options.UseSqlite(configuration.GetConnectionString("PriceBot")));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IProductsProcessing, ProductsProcessing>();
        services.AddScoped<IProductsReprocessingQueue, ProductsReprocessingQueue>();
        //builder.Services.AddHttpClient<ICurrencyApiClient, ExchangeRateApiClient>(options =>
        //{
        //    options.BaseAddress = new Uri(builder.Configuration.GetSection("ExchangeRateApi:Url").Value ?? string.Empty);
        //});
        services.AddHttpClient<ICurrencyApiClient, AbstractApiClient>(options =>
        {
            options.BaseAddress = new Uri(configuration.GetSection("ExchangeRateApi:Url").Value ?? string.Empty);
        });

        return services;
    }
}
