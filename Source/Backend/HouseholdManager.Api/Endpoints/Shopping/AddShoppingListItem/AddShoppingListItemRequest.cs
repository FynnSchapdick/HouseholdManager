namespace HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;

public sealed record AddShoppingListItemRequest(Guid ProductId, int Amount = 1);