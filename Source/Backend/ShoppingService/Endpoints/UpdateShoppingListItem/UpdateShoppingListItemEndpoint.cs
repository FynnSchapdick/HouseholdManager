using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Validation;
using ShoppingService.Data;
using ShoppingService.Domain;

namespace ShoppingService.Endpoints.UpdateShoppingListItem;

public static class UpdateShoppingListItemEndpoint
{
    public static IEndpointRouteBuilder MapRemoveShoppingListItemEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPatch("shoppinglists/{shoppinglistId:guid}/items/{productId:guid}", UpdateShoppingListItem)
            .Accepts<UpdateShoppingListItemRequest>(MediaTypeNames.Application.Json)
            .Produces((int) HttpStatusCode.OK)
            .Produces((int) HttpStatusCode.NotFound)
            .Produces((int) HttpStatusCode.Conflict)
            .Produces((int) HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateShoppingListItemRequest>>()
            .WithTags("ShoppingLists");
        
        return builder;
    }

    private static async Task<IResult> UpdateShoppingListItem([AsParameters] UpdateShoppingListItemParameters parameters, [FromBody] UpdateShoppingListItemRequest request, ShoppingDbContext shoppingDbContext, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingList? shoppingList = await shoppingDbContext
                .ShoppingLists
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == parameters.ShoppinglistId, cancellationToken);

            if (shoppingList is null)
            {
                return Results.NoContent();
            }

            if (!shoppingList.UpdateItem(parameters.ProductId, request.Amount))
            {
                return Results.NotFound();
            }
            
            await shoppingDbContext.SaveChangesAsync(cancellationToken);
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