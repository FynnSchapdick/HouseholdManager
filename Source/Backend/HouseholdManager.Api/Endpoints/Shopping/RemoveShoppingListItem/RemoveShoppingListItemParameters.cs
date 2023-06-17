namespace HouseholdManager.Api.Endpoints.Shopping.RemoveShoppingListItem;

public sealed record RemoveShoppingListItemParameters(Guid ShoppingListId, Guid ProductId);