using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Validation;
using ShoppingService.Data;
using ShoppingService.Domain;

namespace ShoppingService.Endpoints.AddShoppingItem;

public static class AddShoppingItemEndpoint
{
    private const string PostShoppingItemRoute = "shoppinglists/{shoppinglistId:guid}/shoppingitems";
    private const string ShoppingListsSwaggerTag = "ShoppingLists";

    public static void MapPostShoppingItemEndpoint(this WebApplication app)
    {
        app.MapPost(PostShoppingItemRoute, AddShoppingItem)
            .Accepts<AddShoppingItemRequest>(MediaTypeNames.Application.Json)
            .Produces((int) HttpStatusCode.Created)
            .Produces((int) HttpStatusCode.Conflict)
            .Produces((int) HttpStatusCode.BadRequest)
            .Produces((int) HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<AddShoppingItemRequest>>()
            .WithTags(ShoppingListsSwaggerTag);
    }

    private static async Task<IResult> AddShoppingItem(Guid shoppinglistId, AddShoppingItemRequest request, ShoppingContext shoppingContext, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingList? shoppingList = await shoppingContext
                .ShoppingLists
                .AsTracking()
                .Include(x => x.ShoppingItems)
                .FirstOrDefaultAsync(x => x.Id == shoppinglistId, cancellationToken);
        
            if (shoppingList is null)
            {
                return Results.NotFound();
            }

            ShoppingItem? shoppingItem = shoppingList.ShoppingItems.SingleOrDefault(x => x.Ean == request.Ean);
            if (shoppingItem is not null)
            {
                shoppingItem.IncreaseAmount(request.Amount);
                await shoppingContext.SaveChangesAsync(cancellationToken);
                return Results.Ok();
            }
            
            shoppingItem = ShoppingItem.CreateNew(shoppingList.Id, request.Ean, request.Amount);
            shoppingList.ShoppingItems.Add(shoppingItem);
            shoppingContext.ShoppingLists.Update(shoppingList);
            await shoppingContext.SaveChangesAsync(cancellationToken);
            return Results.Ok();
        }
        catch (DbUpdateException dbUpdateException)
        {
            return Results.Conflict(dbUpdateException.InnerException?.Message);
        }
        catch (Exception exception)
        {
            return Results.Problem(new ProblemDetails { Detail = exception.Message, Status = (int) HttpStatusCode.InternalServerError });
        }
    }
}