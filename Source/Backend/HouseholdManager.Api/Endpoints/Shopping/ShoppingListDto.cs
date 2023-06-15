using HouseholdManager.Api.Domain;
using HouseholdManager.Api.Domain.Shopping;

namespace HouseholdManager.Api.Endpoints.Shopping;

public sealed record ShoppingListDto(Guid ShoppingListId, string Name, IEnumerable<ShoppingItemDto> Items)
{
    public static ShoppingListDto FromDomain(ShoppingListAggregate shoppingList)
    {
        return new ShoppingListDto(shoppingList.Id, shoppingList.Name, shoppingList.Items.Select(ShoppingItemDto.FromDomain));
    }
}