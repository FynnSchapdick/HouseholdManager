using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mime;
using HouseholdManager.Api.Data;
using HouseholdManager.Api.Domain;
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
                return Results.NotFound();
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
