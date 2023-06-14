namespace HouseholdManager.Api.Endpoints.Shopping.RemoveShoppingListItem;

public sealed record RemoveShoppingListItemParameters(Guid ShoppinglistId, Guid ProductId);