using HouseholdManager.Domain.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseholdManager.Data.Product.Configuration;

internal sealed class ProductConfigurations : IEntityTypeConfiguration<ProductAggregate>
{
    public void Configure(EntityTypeBuilder<ProductAggregate> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ProductAggregate.Conventions.NAME_MAX_LENGTH);
    }
}