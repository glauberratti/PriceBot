namespace PriceBot.Domain.Product.Repository
{
    public interface IProductRepository
    {
        ValueTask<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product entity);
        Task UpdateAsync(Product entity);
        Task DeleteAsync(Guid id);
        Task<Product> GetRandomAsync();
    }
}
