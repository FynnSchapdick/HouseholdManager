using System.Net;
using HouseholdManager.Data.Shopping;
using HouseholdManager.Domain.Shopping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Http;

namespace HouseholdManager.Api.Endpoints.Shopping.GetShoppingListDetail;

public sealed class GetShoppingListDetailEndpoint : IEndpoint
{
    public const string ENDPOINT_NAME = "GetShoppingListById";

    public static void Configure(IEndpointRouteBuilder builder, string route)
    {
        builder.MapGet(route, GetShoppingList)
            .Produces<ShoppingListDetailDto>()
            .Produces((int)HttpStatusCode.InternalServerError)
            .Produces((int)HttpStatusCode.NotFound)
            .WithName(ENDPOINT_NAME)
            .WithTags("ShoppingLists");
    }

    private static async Task<IResult> GetShoppingList(
        [AsParameters] GetShoppingListDetailParameters parameters,
        ShoppingDbContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            ShoppingListAggregate? shoppingList = await context.ShoppingLists.SingleOrDefaultAsync(x => x.ShoppingListId == parameters.ShoppingListId, cancellationToken: cancellationToken);

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
