using Microsoft.EntityFrameworkCore;
using PriceBot.Domain.Product;
using PriceBot.Infra.Data.Mappings;

namespace PriceBot.Infra.Data.Context
{
    public class DbPriceBotContext : DbContext
    {
        public DbPriceBotContext(DbContextOptions<DbPriceBotContext> options) : base(options) {}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "EntityFramework uses it")]
        DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductMap());
        }
    }
}
