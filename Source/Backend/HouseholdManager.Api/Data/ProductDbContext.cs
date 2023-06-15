using HouseholdManager.Api.Data.Configurations;
using HouseholdManager.Api.Domain;
using HouseholdManager.Api.Domain.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Data;

public sealed class ProductDbContext : DbContext
{
    private readonly IPublishEndpoint _dispatcher;

    public ProductDbContext(DbContextOptions<ProductDbContext> options, IPublishEndpoint dispatcher) : base(options)
    {
        _dispatcher = dispatcher;
    }

    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfigurations());
    }
}
