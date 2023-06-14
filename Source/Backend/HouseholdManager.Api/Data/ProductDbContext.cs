using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Data;

public sealed class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }
}