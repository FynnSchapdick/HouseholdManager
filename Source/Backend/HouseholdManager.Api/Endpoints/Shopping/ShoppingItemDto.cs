﻿using HouseholdManager.Api.Domain;

namespace HouseholdManager.Api.Endpoints.Shopping;

public sealed record ShoppingItemDto(Guid ProductId, int Amount)
{
    public static ShoppingItemDto FromDomain(ShoppingListItem item)
    {
        return new ShoppingItemDto(item.ProductId, item.Amount);
    }
}