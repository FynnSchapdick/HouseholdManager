using Microsoft.AspNetCore.Mvc;

namespace HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;

public sealed record AddShoppingListItemParameters(Guid ShoppingListId, [FromBody] AddShoppingListItemRequest Body);