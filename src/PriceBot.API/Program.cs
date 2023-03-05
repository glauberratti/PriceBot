using Microsoft.EntityFrameworkCore;
using PriceBot.Application.Interfaces;
using PriceBot.Application.Services;
using PriceBot.Domain.Product;
using PriceBot.Domain.Product.Repository;
using PriceBot.Infra.Data.Context;

var builder = WebApplication.CreateBuilder(args);

#region Services
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbPriceBotContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("PriceBot")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
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
app.MapGet("/get-product", async (Guid id, IProductService productService) =>
{
    var product = await productService.GetByIdAsync(id);

    if (product is null)
        return Results.NotFound();

    return Results.Ok(product);
})
.WithOpenApi();

app.MapGet("/get-random-product", async (IProductService productService) =>
{
    var product = await productService.GetRandomAsync();

    return Results.Ok(product);
})
.WithOpenApi();
#endregion

#region DB Seed
// Seed
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
