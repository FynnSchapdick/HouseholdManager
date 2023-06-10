using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingService.Data;
using ShoppingService.Domain;

namespace ShoppingService.Endpoints.GetShoppingList;

public static class GetShoppingListEndpoint
{
    private const string GetShoppingListRoute = "shoppinglists/{shoppinglistId:guid}";
    public const string InternalRouteName = "GetShoppingListById";
    private const string ShoppingListsSwaggerTag = "ShoppingLists";

    public static void MapGetGetShoppingListEndpoint(this WebApplication app)
    {
        app.MapGet(GetShoppingListRoute, GetShoppingList)
            .Produces<ShoppingListDto>(contentType: MediaTypeNames.Application.Json)
            .Produces((int) HttpStatusCode.InternalServerError)
            .Produces((int) HttpStatusCode.NotFound)
            .WithName(InternalRouteName)
            .WithTags(ShoppingListsSwaggerTag);
    }

    private static async Task<IResult> GetShoppingList(Guid shoppinglistId, ShoppingContext shoppingContext, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingList? shoppingList = await shoppingContext
                .ShoppingLists
                .Include(x => x.ShoppingItems)
                .FirstOrDefaultAsync(x => x.Id == shoppinglistId, cancellationToken);

            return shoppingList is null
                ? Results.NotFound(new {ShoppingListId = shoppinglistId})
                : Results.Ok(new ShoppingListDto(shoppingList.Id, shoppingList.Name, shoppingList.ShoppingItems.Select(x => new ShoppingItemDto(x.Ean, x.Amount))));
        }
        catch (Exception exception)
        {
            return Results.Problem(new ProblemDetails
                {Detail = exception.Message, Status = (int) HttpStatusCode.InternalServerError});
        }
    }
}