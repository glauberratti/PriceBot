using PriceBot.Application.Interfaces;
using PriceBot.Domain.Product;
using PriceBot.Domain.Product.Repository;

namespace PriceBot.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            Product? entity = await _productRepository.GetByIdAsync(id);

            return entity;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task<Product> GetRandomAsync()
        {
            return await _productRepository.GetRandomAsync();
        }
    }
}
