using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HouseholdManager.Data.Product.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureProductDbContext(this WebApplicationBuilder builder, string connectionStringName, Action<DbContextOptionsBuilder>? configure = null)
    {
        builder.Services.AddDbContext<ProductDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString(connectionStringName));
            opt.UseSnakeCaseNamingConvention();
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            configure?.Invoke(opt);
        });

        return builder;
    }
}