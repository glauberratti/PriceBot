using PriceBot.Domain.Product;

namespace PriceBot.Application.Interfaces
{
    public interface IProductService
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product entity);
        Task UpdateAsync(Product entity);
        Task DeleteAsync(Guid id);
        Task<Product> GetRandomAsync();
    }
}
