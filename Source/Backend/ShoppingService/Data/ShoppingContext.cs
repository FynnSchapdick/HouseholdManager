using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShoppingService.Data.Options;
using ShoppingService.Domain;

namespace ShoppingService.Data;

public sealed class ShoppingContext : DbContext
{
    private readonly ShoppingDbOptions _contextOptions;
    
    public ShoppingContext(IOptions<ShoppingDbOptions> options)
    {
        _contextOptions = options.Value;
    }
    
    public DbSet<ShoppingList> ShoppingLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql(_contextOptions.ConnectionString);
        builder.UseSnakeCaseNamingConvention();
        builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}