namespace ShoppingService.Endpoints.RemoveShoppingListItem;

public sealed record RemoveShoppingListItemParameters(Guid ShoppinglistId, Guid ProductId);