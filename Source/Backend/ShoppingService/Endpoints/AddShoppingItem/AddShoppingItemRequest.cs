namespace ShoppingService.Endpoints.AddShoppingItem;

public sealed record AddShoppingItemRequest(Guid ProductId, int Amount = 1);