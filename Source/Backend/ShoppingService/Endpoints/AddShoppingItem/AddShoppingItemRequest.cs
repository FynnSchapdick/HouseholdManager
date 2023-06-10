namespace ShoppingService.Endpoints.AddShoppingItem;

public sealed record AddShoppingItemRequest(string Ean, int Amount = 1);