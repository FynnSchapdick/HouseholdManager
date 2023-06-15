namespace HouseholdManager.Api.Endpoints.Shopping.UpdateShoppingListItem;

public sealed record UpdateShoppingListItemParameters(Guid ShoppingListId, Guid ProductId);