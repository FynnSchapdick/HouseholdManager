using ShoppingService.Extensions;
using Throw;

namespace ShoppingService.Domain;

public sealed record ShoppingItem
{
    public required Guid ShoppingListId { get; init; }
    public required string Ean { get; init; }
    public int Amount { get; private set; } = 1;

    public void IncreaseAmount(int amount)
    {
        Amount += amount.Throw().IfLessThan(1);
    }

    public static ShoppingItem CreateNew(Guid shoppingListId, string ean, int amount)
    {
        return new ShoppingItem
        {
            ShoppingListId = shoppingListId,
            Ean = ean.Throw().IfNotEan8Or13(),
            Amount = amount.Throw().IfLessThan(1)
        };
    }
}