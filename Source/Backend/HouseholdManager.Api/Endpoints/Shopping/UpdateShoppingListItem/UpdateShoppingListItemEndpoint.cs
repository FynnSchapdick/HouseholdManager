using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mime;
using HouseholdManager.Domain.Shopping;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Endpoints.Shopping.UpdateShoppingListItem;

public static class UpdateShoppingListItemEndpoint
{
    public static IEndpointRouteBuilder MapUpdateShoppingListItemEndpoint(this IEndpointRouteBuilder builder,[StringSyntax("Route"), RouteTemplate] string route)
    {
        builder.MapPut(route, UpdateShoppingListItem)
            .Accepts<UpdateShoppingListItemRequest>(MediaTypeNames.Application.Json)
            .Produces((int) HttpStatusCode.OK)
            .Produces((int) HttpStatusCode.NotFound)
            .Produces((int) HttpStatusCode.Conflict)
            .Produces((int) HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateShoppingListItemRequest>>()
            .WithTags("ShoppingLists");

        return builder;
    }

    private static async Task<IResult> UpdateShoppingListItem([AsParameters] UpdateShoppingListItemParameters parameters, IShoppingListRepository repository, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingListAggregate? shoppingList = await repository.GetByIdAsync(parameters.ShoppingListId, cancellationToken);

            if (shoppingList is null)
            {
                return Results.NotFound();
            }

            if (!shoppingList.UpdateItem(parameters.ProductId, parameters.Body.Amount))
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
