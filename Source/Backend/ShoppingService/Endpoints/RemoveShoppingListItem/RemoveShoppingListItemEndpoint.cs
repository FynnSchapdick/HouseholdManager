using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingService.Data;
using ShoppingService.Domain;

namespace ShoppingService.Endpoints.RemoveShoppingListItem;

public static class RemoveShoppingListItemEndpoint
{
    public static IEndpointRouteBuilder MapRemoveShoppingListItemEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapDelete("shoppinglists/{shoppinglistId:guid}/items/{productId:guid}", RemoveShoppingListItem)
            .Produces((int) HttpStatusCode.OK)
            .Produces((int) HttpStatusCode.NotFound)
            .Produces((int) HttpStatusCode.Conflict)
            .Produces((int) HttpStatusCode.InternalServerError)
            .WithTags("ShoppingLists");
        
        return builder;
    }
    
    private static async Task<IResult> RemoveShoppingListItem(Guid shoppinglistId, Guid productId, ShoppingDbContext shoppingDbContext, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingList? shoppingList = await shoppingDbContext
                .ShoppingLists
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == shoppinglistId, cancellationToken);

            if (shoppingList is null)
            {
                return Results.NotFound();
            }

            if (shoppingList.RemoveItem(productId))
            {
                await shoppingDbContext.SaveChangesAsync(cancellationToken);
            }
            
            return Results.Ok();
        }
        catch (DbUpdateException dbUpdateException)
        {
            return Results.Problem(new ProblemDetails
            {
                Detail = dbUpdateException.InnerException?.Message ?? dbUpdateException.Message,
                Status = (int) HttpStatusCode.Conflict
            });
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