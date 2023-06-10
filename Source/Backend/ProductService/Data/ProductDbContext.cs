using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductService.Data.Options;
using ProductService.Domain;

namespace ProductService.Data;

public sealed class ProductDbContext : DbContext
{
    private readonly ProductDbOptions _contextOptions;

    public ProductDbContext(IOptions<ProductDbOptions> options)
    {
        _contextOptions = options.Value;
    }

    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql(_contextOptions.ConnectionString);
        builder.UseSnakeCaseNamingConvention();
        builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}