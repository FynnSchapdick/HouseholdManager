namespace ShoppingService.Endpoints.UpdateShoppingListItem;

public sealed record UpdateShoppingListItemParameters(Guid ShoppinglistId, Guid ProductId);