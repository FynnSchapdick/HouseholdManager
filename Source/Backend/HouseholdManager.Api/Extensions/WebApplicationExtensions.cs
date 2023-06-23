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
using Shared.Http;

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
            // build a swagger endpoint for each discovered API version
            app.DescribeApiVersions()
                .Select(description => description.GroupName)
                .ToList()
                .ForEach(groupName => options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant()));
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
            .Map<CreateProductEndpoint>()
            .Map<GetProductEndpoint>("{productId:guid}");

        app.NewVersionedApi("Shopping")
            .MapGroup("api/v{version:apiVersion}/shoppinglists")
            .HasApiVersion(1.0)
            .Map<CreateShoppingListEndpoint>()
            .Map<GetShoppingListsEndpoint>()
            .Map<GetShoppingListDetailEndpoint>("{shoppingListId:guid}")
            .Map<RemoveShoppingListEndpoint>("{shoppingListId:guid}")
            .Map<AddShoppingListItemEndpoint>("{shoppingListId:guid}/items")
            .Map<UpdateShoppingListItemEndpoint>("{shoppingListId:guid}/items/{productId:guid}/amount")
            .Map<RemoveShoppingListItemEndpoint>("{shoppingListId:guid}/items/{productId:guid}");

        return app;
    }
}
