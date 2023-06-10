using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingService.Domain;

namespace ShoppingService.Data.Configurations;

public sealed class ShoppingListConfiguration : IEntityTypeConfiguration<ShoppingList>
{
    public void Configure(EntityTypeBuilder<ShoppingList> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(ShoppingList.NameMaxLength);

        builder.OwnsMany(
            p => p.ShoppingItems, a =>
            {
                a.WithOwner().HasForeignKey(x => x.ShoppingListId);
                a.HasKey(x => new { x.ShoppingListId, x.Ean });
            });
    }
}