using Microsoft.EntityFrameworkCore;
using PriceBot.Infra.Data.Context;

namespace PriceBot.Domain.Product.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbPriceBotContext _dbContext;

        public ProductRepository(DbPriceBotContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async ValueTask<Product?> GetByIdAsync(Guid id)
        {
            Product? entity = await _dbContext.Set<Product>().FindAsync(id);
            if (entity != null)
                _dbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbContext.Set<Product>().AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Product entity)
        {
            await _dbContext.Set<Product>().AddAsync(entity);
        }

        public async Task UpdateAsync(Product entity)
        {
            await Task.Run(() => { _dbContext.Entry(entity).State = EntityState.Modified; });
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContext.Set<Product>().FindAsync(id);
            _dbContext.Set<Product>().Remove(entity!);
        }

        public async Task<Product> GetRandomAsync()
        {
            var count = await _dbContext.Set<Product>().CountAsync();

            Random random = new(72387471);
            var randomNum = random.Next(1, count + 1);

            var randomProduct = _dbContext.Set<Product>()
                .OrderBy(p => p.Id)
                .Skip(randomNum)
                .Take(1)
                .First()!;

            return randomProduct;
        }
    }
}
