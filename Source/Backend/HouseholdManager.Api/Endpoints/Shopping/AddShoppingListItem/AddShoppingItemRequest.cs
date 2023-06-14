namespace HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;

public sealed record AddShoppingItemRequest(Guid ProductId, int Amount = 1);