using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Validation;
using ShoppingService.Data;
using ShoppingService.Domain;
using ShoppingService.Endpoints.GetShoppingList;

namespace ShoppingService.Endpoints.CreateShoppingList;

public static class CreateShoppingListEndpoint
{
    public static IEndpointRouteBuilder MapCreateShoppingListEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("shoppinglists", CreateShoppingList)
            .Accepts<CreateShoppingListRequest>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.Created)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateShoppingListRequest>>()
            .WithTags("ShoppingLists");

        return builder;
    }

    private static async Task<IResult> CreateShoppingList(CreateShoppingListRequest request, ShoppingDbContext shoppingDbContext, CancellationToken cancellationToken)
    {
        try
        {
            var shoppingList = ShoppingList.CreateNew(request.Name);

            shoppingDbContext.Add(shoppingList);
            await shoppingDbContext.SaveChangesAsync(cancellationToken);

            return Results.CreatedAtRoute(GetShoppingListEndpoint.ENDPOINT_NAME, new GetShoppingListParameters(shoppingList.Id));
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