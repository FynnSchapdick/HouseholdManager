using Microsoft.AspNetCore.Mvc;

namespace HouseholdManager.Api.Endpoints.Shopping.UpdateShoppingListItem;

public sealed record UpdateShoppingListItemParameters(Guid ShoppingListId, Guid ProductId, [FromBody] UpdateShoppingListItemRequest Body);
