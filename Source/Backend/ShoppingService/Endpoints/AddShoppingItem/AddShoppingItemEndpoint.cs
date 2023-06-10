using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Validation;
using ShoppingService.Data;
using ShoppingService.Domain;
using ShoppingService.Endpoints.GetShoppingList;

namespace ShoppingService.Endpoints.AddShoppingItem;

public static class AddShoppingItemEndpoint
{
    public static void MapPostShoppingItemEndpoint(this WebApplication app)
    {
        app.MapPost("shoppinglists/{shoppinglistId:guid}/items", AddShoppingItem)
            .Accepts<AddShoppingItemRequest>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.Created)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<AddShoppingItemRequest>>()
            .WithTags("ShoppingLists");
    }

    private static async Task<IResult> AddShoppingItem(Guid shoppinglistId, AddShoppingItemRequest request, ShoppingContext shoppingContext, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingList? shoppingList = await shoppingContext
                .ShoppingLists
                .AsTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == shoppinglistId, cancellationToken);

            if (shoppingList is null)
            {
                return Results.NotFound();
            }

            return shoppingList.Items.SingleOrDefault(x => x.Ean == request.Ean) switch
            {
                { } item => await UpdateItem(item),
                _ => await CreateItem()
            };

            async Task<IResult> CreateItem()
            {
                var item = ShoppingListItem.CreateNew(shoppingList.Id, request.Ean, request.Amount);

                shoppingList.Items.Add(item);
                shoppingContext.ShoppingLists.Update(shoppingList);

                await shoppingContext.SaveChangesAsync(cancellationToken);

                return Results.CreatedAtRoute(GetShoppingListEndpoint.ENDPOINT_NAME, new GetShoppingListParameters(shoppingList.Id));
            }

            async Task<IResult> UpdateItem(ShoppingListItem item)
            {
                item.IncreaseAmountBy(request.Amount);

                await shoppingContext.SaveChangesAsync(cancellationToken);

                return Results.Ok();
            }
        }
        catch (DbUpdateException dbUpdateException)
        {
            return Results.Conflict(dbUpdateException.InnerException?.Message);
        }
        catch (Exception exception)
        {
            return Results.Problem(new ProblemDetails { Detail = exception.Message, Status = (int)HttpStatusCode.InternalServerError });
        }
    }
}
