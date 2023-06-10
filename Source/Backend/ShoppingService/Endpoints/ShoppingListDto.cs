namespace ShoppingService.Endpoints;

public sealed record ShoppingListDto(Guid ShoppingListId, string Name, IEnumerable<ShoppingItemDto> Items);