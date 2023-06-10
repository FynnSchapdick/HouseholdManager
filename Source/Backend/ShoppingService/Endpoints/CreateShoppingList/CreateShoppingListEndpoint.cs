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
    public static void MapPostShoppingListEndpoint(this WebApplication app)
    {
        app.MapPost("shoppinglists", CreateShoppingList)
            .Accepts<CreateShoppingListRequest>(MediaTypeNames.Application.Json)
            .Produces((int) HttpStatusCode.Created)
            .Produces((int) HttpStatusCode.Conflict)
            .Produces((int) HttpStatusCode.BadRequest)
            .Produces((int) HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateShoppingListRequest>>()
            .WithTags("ShoppingLists");
    }

    private static async Task<IResult> CreateShoppingList(CreateShoppingListRequest request, ShoppingDbContext shoppingDbContext, CancellationToken cancellationToken)
    {
        try
        {
            var shoppingList = ShoppingList.CreateNew(request.Name);

            await shoppingDbContext.AddAsync(shoppingList, cancellationToken);
            await shoppingDbContext.SaveChangesAsync(cancellationToken);

            return Results.CreatedAtRoute(GetShoppingListEndpoint.ENDPOINT_NAME, new { ShoppingListId = shoppingList.Id });
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
