using PriceBot.Application.Interfaces;
using PriceBot.Domain.Product;
using PriceBot.Domain.Product.Repository;
using PriceBot.Domain.SharedKernel.Enums;

namespace PriceBot.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrencyService _currencyService;

        public ProductService(IProductRepository productRepository, ICurrencyService currencyService)
        {
            _productRepository = productRepository;
            _currencyService = currencyService;
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
            product.Id = Guid.NewGuid();
            await GetAndUpdateUsdCurrency(product);
            await GetAndUpdateEurCurrency(product);
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

        private async Task GetAndUpdateUsdCurrency(Product product)
        {
            decimal usdValue = await _currencyService.GetCurrencyValue(Currency.USD);
            product.UpdateUsdCurrency(usdValue);
        }

        private async Task GetAndUpdateEurCurrency(Product product)
        {
            decimal eurValue = await _currencyService.GetCurrencyValue(Currency.EUR);
            product.UpdateEurCurrency(eurValue);
        }
    }
}
