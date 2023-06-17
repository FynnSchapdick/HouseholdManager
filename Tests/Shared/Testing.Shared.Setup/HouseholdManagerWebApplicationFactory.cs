using System.Data.Common;
using HouseholdManager.Data.Product;
using HouseholdManager.Data.Shopping;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit.Abstractions;

namespace Testing.Shared.Setup;

public sealed class HouseholdManagerWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly ITestOutputHelper _outputHelper;

    public HouseholdManagerWebApplicationFactory(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseSerilog((_, configuration) =>
        {
            configuration.WriteTo.TestOutput(_outputHelper);
        });

        builder.ConfigureServices(services =>
        {
            services.AddMassTransitTestHarness();
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ShoppingDbContext>));
            services.RemoveAll(typeof(DbContextOptions<ProductDbContext>));

            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=file:memdb1?mode=memory&cache=shared");
                connection.Open();

                return connection;
            });

            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=file:memdb2?mode=memory&cache=shared");
                connection.Open();

                return connection;
            });

            services.AddDbContext<ShoppingDbContext>((provider, options) =>
            {
                var connection = provider.GetServices<DbConnection>().First();
                options.UseSqlite(connection);
            });

            services.AddDbContext<ProductDbContext>((provider, options) =>
            {
                var connection = provider.GetServices<DbConnection>().Skip(1).First();
                options.UseSqlite(connection);
            });
        });

        IHost host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<ShoppingDbContext>().Database.EnsureCreated();
        scope.ServiceProvider.GetRequiredService<ProductDbContext>().Database.EnsureCreated();

        return host;
    }
}
