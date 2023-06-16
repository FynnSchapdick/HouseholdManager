using System.Diagnostics.CodeAnalysis;
using System.Net;
using HouseholdManager.Domain.Shopping;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Endpoints.Shopping.RemoveShoppingListItem;

public static class RemoveShoppingListItemEndpoint
{
    public static IEndpointRouteBuilder MapRemoveShoppingListItemEndpoint(this IEndpointRouteBuilder builder, [StringSyntax("Route"), RouteTemplate] string route)
    {
        builder.MapDelete(route, RemoveShoppingListItem)
            .Produces((int) HttpStatusCode.OK)
            .Produces((int) HttpStatusCode.NotFound)
            .Produces((int) HttpStatusCode.Conflict)
            .Produces((int) HttpStatusCode.InternalServerError)
            .WithTags("ShoppingLists");

        return builder;
    }

    private static async Task<IResult> RemoveShoppingListItem([AsParameters] RemoveShoppingListItemParameters parameters, IShoppingListRepository repository, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingListAggregate? shoppingList = await repository.GetByIdAsync(parameters.ProductId, cancellationToken);

            if (shoppingList is null)
            {
                return Results.NotFound();
            }

            if (!shoppingList.RemoveItem(parameters.ProductId))
            {
                return Results.NotFound();
            }

            await repository.SaveAsync(shoppingList, cancellationToken);
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
