using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShoppingService.Data.Options;
using ShoppingService.Domain;

namespace ShoppingService.Data;

public sealed class ShoppingDbContext : DbContext
{
    public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options) : base(options)
    {
    }

    public DbSet<ShoppingList> ShoppingLists { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingDbContext).Assembly);
    }
}