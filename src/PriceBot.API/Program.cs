using Microsoft.EntityFrameworkCore;
using PriceBot.Application.Interfaces;
using PriceBot.Application.Mappings;
using PriceBot.Application.Services;
using PriceBot.Application.ViewModels;
using PriceBot.CrossCutting.CurrencyApi;
using PriceBot.CrossCutting.Settings;
using PriceBot.Domain.Product;
using PriceBot.Domain.Product.Repository;
using PriceBot.Domain.Queue;
using PriceBot.Infra.Data.Context;
using PriceBot.Infra.Data.Queue;

var builder = WebApplication.CreateBuilder(args);

#region Services
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Movo to configuration
builder.Services.Configure<Settings>(options =>
{
    options.CurrencyApi.Url = builder.Configuration.GetSection("ExchangeRateApi:Url").Value ?? string.Empty;
    options.CurrencyApi.EndPointLatest = builder.Configuration.GetSection("ExchangeRateApi:EndPointLatest").Value ?? string.Empty;
    options.CurrencyApi.Key = builder.Configuration.GetSection("ExchangeRateApi:Key").Value ?? string.Empty;
    options.RabbitMQConfig.VirtualHost = builder.Configuration.GetSection("RabbitMQ:VirtualHost").Value ?? string.Empty;
    options.RabbitMQConfig.HostName = builder.Configuration.GetSection("RabbitMQ:HostName").Value ?? string.Empty;
    options.RabbitMQConfig.Port = Convert.ToInt16(builder.Configuration.GetSection("RabbitMQ:Port").Value ?? string.Empty);
    options.RabbitMQConfig.UserName = builder.Configuration.GetSection("RabbitMQ:UserName").Value ?? string.Empty;
    options.RabbitMQConfig.Password = builder.Configuration.GetSection("RabbitMQ:Password").Value ?? string.Empty;
    options.RabbitMQConfig.ProductsReprocessingQueue = builder.Configuration.GetSection("RabbitMQ:ProductsReprocessingQueue").Value ?? string.Empty;
});

builder.Services.AddDbContext<DbPriceBotContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("PriceBot")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IProductsProcessing, ProductsProcessing>();
builder.Services.AddScoped<IProductsReprocessingQueue, ProductsReprocessingQueue>();

//builder.Services.AddHttpClient<ICurrencyApiClient, ExchangeRateApiClient>(options =>
//{
//    options.BaseAddress = new Uri(builder.Configuration.GetSection("ExchangeRateApi:Url").Value ?? string.Empty);
//});

builder.Services.AddHttpClient<ICurrencyApiClient, AbstractApiClient>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetSection("ExchangeRateApi:Url").Value ?? string.Empty);
});

#endregion

var app = builder.Build();

#region Middlewares
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
#endregion

#region Controllers
// Controllers
// TODO: Move to a configuration
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
    await productsProcessing.ReprocessUsdValueProduct();
});
#endregion

#region DB Seed
// Seed

// TODO: Move to Infra.Data
using (var scope = app.Services.CreateScope())
{
    List<Product> products = new();
    var dbContext = scope.ServiceProvider.GetService<DbPriceBotContext>();

    if (dbContext is not null)
    {
        int c = dbContext.Set<Product>().Count();

        if (c == 0)
        {
            foreach (var i in Enumerable.Range(1, 1000))
            {
                var f = new Bogus.Faker();

                Product product = new()
                {
                    Id = Guid.NewGuid(),
                    Name = f.Commerce.ProductName(),
                    BRLValue = Convert.ToDecimal(f.Commerce.Price())
                };
                products.Add(product);
            }

            dbContext.AddRange(products);
            dbContext.SaveChanges();
        }
    }
}
#endregion

app.Run();
