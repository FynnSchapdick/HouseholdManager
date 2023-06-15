namespace HouseholdManager.Api.Endpoints.Shopping.GetShoppingLists;

public sealed record GetShoppingListsResponse(IEnumerable<ShoppingListDto> ShoppingLists);