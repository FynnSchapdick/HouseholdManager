using System.Net;
using System.Net.Mime;
using HouseholdManager.Api.Endpoints.Shopping.GetShoppingListDetail;
using HouseholdManager.Domain.Shopping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Http;

namespace HouseholdManager.Api.Endpoints.Shopping.CreateShoppingList;

public sealed class CreateShoppingListEndpoint : IEndpoint
{
    public static void Configure(IEndpointRouteBuilder builder, string route)
    {
        builder.MapPost(route, CreateShoppingList)
            .Accepts<CreateShoppingListRequest>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.Created)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateShoppingListRequest>>()
            .WithTags("ShoppingLists");
    }

    private static async Task<IResult> CreateShoppingList(CreateShoppingListRequest request, IShoppingListRepository repository, CancellationToken cancellationToken)
    {
        try
        {
            var shoppingList = ShoppingListAggregate.CreateNew(request.Name);
            await repository.SaveNewAsync(shoppingList, cancellationToken);

            return Results.CreatedAtRoute(GetShoppingListDetailEndpoint.ENDPOINT_NAME, new GetShoppingListDetailParameters(shoppingList.ShoppingListId));
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
