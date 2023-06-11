using Throw;

namespace ShoppingService.Domain;

public sealed record ShoppingListItem
{
    public Guid ShoppingListId { get; private init; }
    public Guid ProductId { get; private init; }
    public int Amount { get; private set; } = 1;

    public static ShoppingListItem CreateNew(Guid shoppingListId, Guid productId, int amount)
    {
        return new ShoppingListItem
        {
            ShoppingListId = shoppingListId.Throw().IfDefault(),
            ProductId = productId.Throw().IfDefault(),
            Amount = amount.Throw().IfLessThan(1)
        };
    }

    public void IncreaseAmountBy(int amount)
    {
        Amount += amount.Throw().IfLessThan(1);
    }
}