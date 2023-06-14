using HouseholdManager.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Data;

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