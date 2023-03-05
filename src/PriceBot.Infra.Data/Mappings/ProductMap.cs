using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceBot.Domain.Product;

namespace PriceBot.Infra.Data.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.BRLValue).IsRequired().HasDefaultValue(0).HasPrecision(28, 3);
            builder.Property(p => p.USDValue).IsRequired().HasDefaultValue(0).HasPrecision(28, 3);
            builder.Property(p => p.EURValue).IsRequired().HasDefaultValue(0).HasPrecision(28, 3);
        }
    }
}
