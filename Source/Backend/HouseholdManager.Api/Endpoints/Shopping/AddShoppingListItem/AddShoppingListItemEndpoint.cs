using System.Net;
using System.Net.Mime;
using HouseholdManager.Api.Data;
using HouseholdManager.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Validation;

namespace HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;

public static class AddShoppingListItemEndpoint
{
    public static IEndpointRouteBuilder MapAddShoppingListItemEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("shoppinglists/{shoppinglistId:guid}/items", AddShoppingListItem)
            .Accepts<AddShoppingItemRequest>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.Created)
            .Produces((int) HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<AddShoppingItemRequest>>()
            .WithTags("ShoppingLists");

        return builder;
    }

    private static async Task<IResult> AddShoppingListItem(Guid shoppinglistId, AddShoppingItemRequest request, ShoppingDbContext shoppingDbContext, CancellationToken cancellationToken)
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

            shoppingList.AddItem(request.ProductId, request.Amount);
            
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