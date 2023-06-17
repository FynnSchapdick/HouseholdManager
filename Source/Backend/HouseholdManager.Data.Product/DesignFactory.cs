using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HouseholdManager.Data.Product;

public sealed class DesignFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ProductDbContext>();
        builder.UseNpgsql();
        builder.UseSnakeCaseNamingConvention();
        return new ProductDbContext(builder.Options, null!);
    }
}
