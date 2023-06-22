using HouseholdManager.Domain.Shopping;
using JetBrains.Annotations;

namespace HouseholdManager.Api.Endpoints.Shopping.GetShoppingListDetail;

[UsedImplicitly]
public sealed record ShoppingListDetailDto(Guid ShoppingListId, string Name, IEnumerable<ShoppingItemDto> Items)
{
    public static ShoppingListDetailDto FromDomain(ShoppingListAggregate shoppingList)
    {
        return new ShoppingListDetailDto(shoppingList.ShoppingListId, shoppingList.Name, shoppingList.Items.Select(ShoppingItemDto.FromDomain));
    }
}
