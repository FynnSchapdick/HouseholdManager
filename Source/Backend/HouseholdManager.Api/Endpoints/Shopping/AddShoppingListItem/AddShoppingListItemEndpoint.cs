using System.Net;
using System.Net.Mime;
using HouseholdManager.Api.Endpoints.Shopping.GetShoppingListDetail;
using HouseholdManager.Domain.Shopping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Http;

namespace HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;

public sealed class AddShoppingListItemEndpoint : IEndpoint
{
    public static void Configure(IEndpointRouteBuilder builder, string route)
    {
        builder.MapPost(route, AddShoppingListItem)
            .Accepts<AddShoppingListItemRequest>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.Created)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<AddShoppingListItemParameters>>()
            .WithTags("ShoppingLists");

    }

    private static async Task<IResult> AddShoppingListItem([AsParameters] AddShoppingListItemParameters parameters, IShoppingListRepository repository, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingListAggregate? shoppingList = await repository.GetByIdAsync(parameters.ShoppingListId, cancellationToken);

            if (shoppingList is null)
            {
                return Results.NotFound();
            }

            bool isNewItem = shoppingList.AddItem(parameters.Body.ProductId, parameters.Body.Amount);

            await repository.SaveAsync(shoppingList, cancellationToken);

            return isNewItem
                ? Results.CreatedAtRoute(GetShoppingListDetailEndpoint.ENDPOINT_NAME, new GetShoppingListDetailParameters(parameters.ShoppingListId))
                : Results.Ok();
        }
        catch (DbUpdateException dbUpdateException)
        {
            return Results.Problem(new ProblemDetails
            {
                Detail = dbUpdateException.InnerException?.Message ?? dbUpdateException.Message,
                Status = (int)HttpStatusCode.Conflict
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
