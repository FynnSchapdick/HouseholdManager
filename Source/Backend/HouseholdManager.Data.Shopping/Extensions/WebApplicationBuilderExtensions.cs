using HouseholdManager.Domain.Shopping;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HouseholdManager.Data.Shopping.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureShoppingDbContext(this WebApplicationBuilder builder, string connectionStringName, Action<DbContextOptionsBuilder>? configure = null)
    {
        builder.Services.AddDbContext<ShoppingDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString(connectionStringName));
            opt.UseSnakeCaseNamingConvention();
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            configure?.Invoke(opt);
        });

        builder.Services.AddTransient<IShoppingListRepository>(x => x.GetRequiredService<ShoppingDbContext>());

        return builder;
    }
}