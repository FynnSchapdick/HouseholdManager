using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HouseholdManager.Data.Shopping;

public sealed class DesignFactory : IDesignTimeDbContextFactory<ShoppingDbContext>
{
    public ShoppingDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ShoppingDbContext>();
        builder.UseNpgsql();
        builder.UseSnakeCaseNamingConvention();
        return new ShoppingDbContext(builder.Options, null!, null!);
    }
}
