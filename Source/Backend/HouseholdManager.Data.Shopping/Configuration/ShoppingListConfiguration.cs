using HouseholdManager.Domain.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseholdManager.Data.Shopping.Configuration;

public sealed class ShoppingListConfiguration : IEntityTypeConfiguration<ShoppingListAggregate>
{
    public void Configure(EntityTypeBuilder<ShoppingListAggregate> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(ShoppingListAggregate.Conventions.NAME_MAX_LENGTH);

        builder.Ignore(x => x.Events);

        builder.OwnsMany(p => p.Items, item =>
        {
            item.WithOwner().HasForeignKey(x => x.ShoppingListId);
            item.HasKey(x => new { x.ShoppingListId, x.ProductId });
            item.Property(x => x.ProductId).ValueGeneratedNever();
            item.OwnsOne(x => x.ProductInfo);
        });

        builder.Navigation(x => x.Items).AutoInclude();
    }
}