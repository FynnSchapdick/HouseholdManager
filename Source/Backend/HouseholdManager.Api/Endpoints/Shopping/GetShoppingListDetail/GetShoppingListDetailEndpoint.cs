using System.Diagnostics.CodeAnalysis;
using System.Net;
using HouseholdManager.Api.Data;
using HouseholdManager.Api.Domain;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Endpoints.Shopping.GetShoppingListDetail;

public static class GetShoppingListDetailEndpoint
{
    public const string ENDPOINT_NAME = "GetShoppingListById";

    public static IEndpointRouteBuilder MapGetShoppingListDetailEndpoint(this IEndpointRouteBuilder builder, [StringSyntax("Route"), RouteTemplate] string route)
    {
        builder.MapGet(route, GetShoppingList)
            .Produces<ShoppingListDetailDto>()
            .Produces((int)HttpStatusCode.InternalServerError)
            .Produces((int)HttpStatusCode.NotFound)
            .WithName(ENDPOINT_NAME)
            .WithTags("ShoppingLists");

        return builder;
    }

    private static async Task<IResult> GetShoppingList(
        [AsParameters] GetShoppingListDetailParameters parameters,
        ShoppingDbContext shoppingDbContext,
        CancellationToken cancellationToken)
    {
        try
        {
            ShoppingList? shoppingList = await shoppingDbContext
                .ShoppingLists
                .FirstOrDefaultAsync(x => x.Id == parameters.ShoppingListId, cancellationToken);

            return shoppingList is null
                ? Results.NotFound(new { parameters.ShoppingListId })
                : Results.Ok(ShoppingListDetailDto.FromDomain(shoppingList));
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
