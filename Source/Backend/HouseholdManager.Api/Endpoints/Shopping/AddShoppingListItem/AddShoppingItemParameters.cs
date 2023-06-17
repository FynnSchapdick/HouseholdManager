using Microsoft.AspNetCore.Mvc;

namespace HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;

public sealed record AddShoppingItemParameters(Guid ShoppingListId, [FromBody] AddShoppingListItemRequest Body);
