using HouseholdManager.Api.Domain;
using HouseholdManager.Api.Domain.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseholdManager.Api.Data.Configurations;

public sealed class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Product.Conventions.NAME_MAX_LENGTH);
    }
}