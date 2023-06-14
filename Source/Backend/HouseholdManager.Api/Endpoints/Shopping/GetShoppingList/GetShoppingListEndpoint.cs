using System.Diagnostics.CodeAnalysis;
using System.Net;
using HouseholdManager.Api.Data;
using HouseholdManager.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Endpoints.Shopping.GetShoppingList;

public static class GetShoppingListEndpoint
{
    public const string ENDPOINT_NAME = "GetShoppingListById";

    public static IEndpointRouteBuilder MapGetShoppingListEndpoint(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string route)
    {
        builder.MapGet(route, GetShoppingList)
            .Produces<ShoppingListDto>()
            .Produces((int)HttpStatusCode.InternalServerError)
            .Produces((int)HttpStatusCode.NotFound)
            .WithName(ENDPOINT_NAME)
            .WithTags("ShoppingLists");

        return builder;
    }

    private static async Task<IResult> GetShoppingList(
        [AsParameters] GetShoppingListParameters parameters,
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
                : Results.Ok(ShoppingListDto.FromDomain(shoppingList));
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
