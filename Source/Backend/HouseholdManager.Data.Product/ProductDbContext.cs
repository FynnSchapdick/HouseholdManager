using HouseholdManager.Domain.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Data.Product;

public sealed class ProductDbContext : DbContext
{
    private readonly IPublishEndpoint _dispatcher;

    public ProductDbContext(DbContextOptions<ProductDbContext> options, IPublishEndpoint dispatcher) : base(options)
    {
        _dispatcher = dispatcher;
    }

    public DbSet<ProductAggregate> Products { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }
}
