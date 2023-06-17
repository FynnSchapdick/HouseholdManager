﻿using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mime;
using HouseholdManager.Api.Endpoints.Shopping.GetShoppingListDetail;
using HouseholdManager.Domain.Shopping;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;

public static class AddShoppingListItemEndpoint
{
    public static IEndpointRouteBuilder MapAddShoppingListItemEndpoint(this IEndpointRouteBuilder builder, [StringSyntax("Route"), RouteTemplate] string route)
    {
        builder.MapPost(route, AddShoppingListItem)
            .Accepts<AddShoppingItemRequest>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.Created)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<AddShoppingItemRequest>>()
            .WithTags("ShoppingLists");

        return builder;
    }

    private static async Task<IResult> AddShoppingListItem(Guid shoppingListId, AddShoppingItemRequest request, IShoppingListRepository repository, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingListAggregate? shoppingList = await repository.GetByIdAsync(shoppingListId, cancellationToken);

            if (shoppingList is null)
            {
                return Results.NotFound();
            }

            bool newItem = shoppingList.AddItem(request.ProductId, request.Amount);

            await repository.SaveAsync(shoppingList, cancellationToken);

            return newItem
                ? Results.CreatedAtRoute(GetShoppingListDetailEndpoint.ENDPOINT_NAME, new GetShoppingListDetailParameters(shoppingListId))
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