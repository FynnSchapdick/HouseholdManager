using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mime;
using HouseholdManager.Api.Data;
using HouseholdManager.Api.Endpoints.Shopping.GetShoppingListDetail;
using HouseholdManager.Domain.Shopping;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Endpoints.Shopping.CreateShoppingList;

public static class CreateShoppingListEndpoint
{
    public static IEndpointRouteBuilder MapCreateShoppingListEndpoint(this IEndpointRouteBuilder builder, [StringSyntax("Route"), RouteTemplate] string route)
    {
        builder.MapPost(route, CreateShoppingList)
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
            var shoppingList = ShoppingListAggregate.CreateNew(request.Name);

            shoppingDbContext.Add(shoppingList);
            await shoppingDbContext.SaveChangesAsync(cancellationToken);

            return Results.CreatedAtRoute(GetShoppingListDetailEndpoint.ENDPOINT_NAME, new GetShoppingListDetailParameters(shoppingList.Id));
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
