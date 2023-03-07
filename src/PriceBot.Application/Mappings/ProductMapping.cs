using PriceBot.Application.ViewModels;
using PriceBot.Domain.Product;

namespace PriceBot.Application.Mappings;

public static class ProductMapping
{
    public static Product Map(this ProductRequestVM productRequestVM)
    {
        return new Product()
        {
            Name = productRequestVM.Name,
            BRLValue = productRequestVM.BRLValue,
        };
    }

    public static ProductResponseVM Map(this Product product)
    {
        return new ProductResponseVM()
        {
            Id = product.Id,
            Name = product.Name,
            BRLValue = product.BRLValue,
            USDValue = product.USDValue,
            EURValue = product.EURValue,
        };
    }
}
