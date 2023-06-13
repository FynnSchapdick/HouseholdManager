using Asp.Versioning.ApiExplorer;
using Serilog;
using ShoppingService.Data;
using ShoppingService.Endpoints.AddShoppingListItem;
using ShoppingService.Endpoints.CreateShoppingList;
using ShoppingService.Endpoints.GetShoppingList;
using ShoppingService.Endpoints.RemoveShoppingListItem;

namespace ShoppingService.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        app.NewVersionedApi("ShoppingLists")
            .MapGroup("/v{version:apiVersion}")
            .HasApiVersion(1.0)
            .MapCreateShoppingListEndpoint()
            .MapGetShoppingListEndpoint()
            .MapAddShoppingListItemEndpoint()
            .MapRemoveShoppingListItemEndpoint();

        app.Services.CreateScope().ServiceProvider.GetRequiredService<ShoppingDbContext>().Database.EnsureCreated();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

            // build a swagger endpoint for each discovered API version
            foreach (ApiVersionDescription description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                string name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });

        return app;
    }
}