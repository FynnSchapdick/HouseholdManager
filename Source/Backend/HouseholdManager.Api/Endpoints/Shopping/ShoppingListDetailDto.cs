using HouseholdManager.Api.Domain.Shopping;

namespace HouseholdManager.Api.Endpoints.Shopping;

public sealed record ShoppingListDetailDto(Guid ShoppingListId, string Name, IEnumerable<ShoppingItemDto> Items)
{
    public static ShoppingListDetailDto FromDomain(ShoppingListAggregate shoppingList)
    {
        return new ShoppingListDetailDto(shoppingList.Id, shoppingList.Name, shoppingList.Items.Select(ShoppingItemDto.FromDomain));
    }
}