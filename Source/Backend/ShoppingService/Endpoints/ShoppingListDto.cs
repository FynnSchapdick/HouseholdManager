using ShoppingService.Domain;

namespace ShoppingService.Endpoints;

public sealed record ShoppingListDto(Guid ShoppingListId, string Name, IEnumerable<ShoppingItemDto> Items)
{
    public static ShoppingListDto FromDomain(ShoppingList shoppingList)
    {
        return new ShoppingListDto(shoppingList.Id, shoppingList.Name, shoppingList.Items.Select(ShoppingItemDto.FromDomain));
    }
}