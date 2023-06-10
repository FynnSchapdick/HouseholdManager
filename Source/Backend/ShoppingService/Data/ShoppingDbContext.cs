using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShoppingService.Data.Options;
using ShoppingService.Domain;

namespace ShoppingService.Data;

public sealed class ShoppingDbContext : DbContext
{
    private readonly ShoppingDbOptions _contextOptions;

    public ShoppingDbContext(IOptions<ShoppingDbOptions> options)
    {
        _contextOptions = options.Value;
    }

    public DbSet<ShoppingList> ShoppingLists { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql(_contextOptions.ConnectionString);
        builder.UseSnakeCaseNamingConvention();
        builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}
