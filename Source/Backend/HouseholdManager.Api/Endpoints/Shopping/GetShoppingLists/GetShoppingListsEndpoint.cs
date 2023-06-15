using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mime;
using HouseholdManager.Api.Data;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Endpoints.Shopping.GetShoppingLists;

public static class GetShoppingListsEndpoint
{
    public static IEndpointRouteBuilder MapGetShoppingListsEndpoint(this IEndpointRouteBuilder builder, [StringSyntax("Route"), RouteTemplate] string route)
    {
        builder.MapGet(route, GetShoppingLists)
            .Produces<GetShoppingListsResponse>(contentType: MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.InternalServerError)
            .WithTags("ShoppingLists");

        return builder;
    }

    private static async Task<IResult> GetShoppingLists(ShoppingDbContext shoppingDbContext, CancellationToken cancellationToken)
    {
        try
        {
            List<ShoppingListDto> shoppingLists = await shoppingDbContext
                .ShoppingLists
                .Select(shoppingList => new ShoppingListDto(shoppingList.Id, shoppingList.Name))
                .ToListAsync(cancellationToken);

            return Results.Ok(new GetShoppingListsResponse(shoppingLists));
        }
        catch (Exception exception) when (exception is not ArgumentException)
        {
            return Results.Problem(new ProblemDetails
            {
                Detail = exception.Message,
                Status = (int)HttpStatusCode.InternalServerError
            });
        }
    }
}