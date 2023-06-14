﻿using Asp.Versioning.ApiExplorer;
using HouseholdManager.Api.Endpoints.Products.CreateProduct;
using HouseholdManager.Api.Endpoints.Products.GetProduct;
using HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;
using HouseholdManager.Api.Endpoints.Shopping.CreateShoppingList;
using HouseholdManager.Api.Endpoints.Shopping.GetShoppingList;
using HouseholdManager.Api.Endpoints.Shopping.RemoveShoppingListItem;
using Serilog;

namespace HouseholdManager.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        app.NewVersionedApi("Products")
            .MapGroup("api/v{version:apiVersion}/products")
            .HasApiVersion(1.0)
            .MapCreateProductEndpoint("/")
            .MapGetProductEndpoint("{productId:guid}");

        app.NewVersionedApi("Shopping")
            .MapGroup("api/v{version:apiVersion}/shoppinglists")
            .HasApiVersion(1.0)
            .MapCreateShoppingListEndpoint("/")
            .MapGetShoppingListEndpoint("{shoppingListId:guid}")
            .MapAddShoppingListItemEndpoint("{shoppingListId:guid}/items")
            .MapRemoveShoppingListItemEndpoint("{shoppingListId:guid}/items/{productId:guid}");

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
