using Asp.Versioning.ApiExplorer;
using HouseholdManager.Api.Endpoints.Products.CreateProduct;
using HouseholdManager.Api.Endpoints.Products.GetProduct;
using HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;
using HouseholdManager.Api.Endpoints.Shopping.CreateShoppingList;
using HouseholdManager.Api.Endpoints.Shopping.GetShoppingListDetail;
using HouseholdManager.Api.Endpoints.Shopping.GetShoppingLists;
using HouseholdManager.Api.Endpoints.Shopping.RemoveShoppingList;
using HouseholdManager.Api.Endpoints.Shopping.RemoveShoppingListItem;
using HouseholdManager.Api.Endpoints.Shopping.UpdateShoppingListItem;
using HouseholdManager.Data.Product;
using HouseholdManager.Data.Shopping;
using Serilog;

namespace HouseholdManager.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }

    public static WebApplication UseVersionedSwagger(this WebApplication app)
    {
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

    public static WebApplication UseDevelopmentConfiguration(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<ShoppingDbContext>().Database.EnsureCreated();
        scope.ServiceProvider.GetRequiredService<ProductDbContext>().Database.EnsureCreated();
        return app;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        app.NewVersionedApi("Products")
            .MapGroup("api/v{version:apiVersion}/products")
            .HasApiVersion(1.0)
            .MapCreateProductEndpoint("/")
            .MapGetProductEndpoint("{productId:guid}");

        app.NewVersionedApi("Shopping")
            .MapGroup("api/v{version:apiVersion}/shoppinglists")
            .HasApiVersion(1.0)
            .MapCreateShoppingListEndpoint("/")
            .MapGetShoppingListsEndpoint("/")
            .MapGetShoppingListDetailEndpoint("{shoppingListId:guid}")
            .MapRemoveShoppingListEndpoint("{shoppingListId:guid}")
            .MapAddShoppingListItemEndpoint("{shoppingListId:guid}/items")
            .MapUpdateShoppingListItemEndpoint("{shoppingListId:guid}/items/{productId:guid}/amount")
            .MapRemoveShoppingListItemEndpoint("{shoppingListId:guid}/items/{productId:guid}");

        return app;
    }
}