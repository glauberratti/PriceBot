using PriceBot.Domain.Product;
using PriceBot.Infra.Data.Context;

namespace PriceBot.API.Configurations;

public static class SeedConfiguration
{
    public static void Seed(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

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
}
