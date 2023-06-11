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
            .HasMaxLength(ShoppingList.Conventions.NAME_MAX_LENGTH);

        builder.OwnsMany(p => p.Items, item =>
        {
            item.WithOwner().HasForeignKey(x => x.ShoppingListId);
            item.HasKey(x => new { x.ShoppingListId, x.ProductId });
            item.Property(x => x.ProductId).ValueGeneratedNever();
        });

        builder.Navigation(x => x.Items).AutoInclude();
    }
}