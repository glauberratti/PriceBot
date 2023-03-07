namespace PriceBot.Application.ViewModels;

public class ProductResponseVM : ProductRequestVM
{
    public Guid Id { get; set; }
    public decimal USDValue { get; set; }
    public decimal EURValue { get; set; }
}
