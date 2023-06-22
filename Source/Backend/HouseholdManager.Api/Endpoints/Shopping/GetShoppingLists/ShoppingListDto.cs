using JetBrains.Annotations;

namespace HouseholdManager.Api.Endpoints.Shopping.GetShoppingLists;

[UsedImplicitly]
public sealed record ShoppingListDto(Guid ShoppingListId, string Name);
