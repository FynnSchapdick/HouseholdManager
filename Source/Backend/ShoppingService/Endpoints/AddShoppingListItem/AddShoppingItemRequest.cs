namespace ShoppingService.Endpoints.AddShoppingListItem;

public sealed record AddShoppingItemRequest(Guid ProductId, int Amount = 1);