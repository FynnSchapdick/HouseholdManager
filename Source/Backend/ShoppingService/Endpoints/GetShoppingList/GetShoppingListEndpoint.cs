using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingService.Data;
using ShoppingService.Domain;

namespace ShoppingService.Endpoints.GetShoppingList;

public static class GetShoppingListEndpoint
{
    public const string ENDPOINT_NAME = "GetShoppingListById";

    public static void MapGetGetShoppingListEndpoint(this WebApplication app)
    {
        app.MapGet("shoppinglists/{shoppinglistId:guid}", GetShoppingList)
            .Produces<ShoppingListDto>()
            .Produces((int)HttpStatusCode.InternalServerError)
            .Produces((int)HttpStatusCode.NotFound)
            .WithName(ENDPOINT_NAME)
            .WithTags("ShoppingLists");
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
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == parameters.ShoppingListId, cancellationToken);

            return shoppingList is null
                ? Results.NotFound(new { parameters.ShoppingListId })
                : Results.Ok(new ShoppingListDto(shoppingList.Id, shoppingList.Name, shoppingList.Items.Select(x => new ShoppingItemDto(x.Ean, x.Amount))));
        }
        catch (Exception exception)
        {
            return Results.Problem(new ProblemDetails
                { Detail = exception.Message, Status = (int)HttpStatusCode.InternalServerError });
        }
    }
}
