using Throw;

namespace HouseholdManager.Domain.Shopping.ValueObjects;

public sealed record ShoppingListItem
{
    public Guid ShoppingListId { get; }
    public Guid ProductId { get; }
    public int Amount { get; private set; }

    public ProductInfo? ProductInfo { get; private set; }

    internal ShoppingListItem(Guid shoppingListId, Guid productId, int amount)
    {
        ShoppingListId = shoppingListId.Throw().IfDefault();
        ProductId = productId.Throw().IfDefault();
        Amount = amount.Throw().IfLessThan(Conventions.MIN_AMOUNT);
    }

    public static ShoppingListItem CreateNew(Guid shoppingListId, Guid productId, int amount)
    {
        return new ShoppingListItem(shoppingListId, productId, amount);
    }

    public void IncreaseAmountBy(int amount)
    {
        Amount += amount.Throw().IfLessThan(Conventions.MIN_AMOUNT);
    }

    public void SetAmount(int amount)
    {
        Amount = amount.Throw().IfLessThan(Conventions.MIN_AMOUNT);
    }

    public void SetProductInfo(ProductInfo productInfo)
    {
        ProductInfo = productInfo.ThrowIfNull();
    }

    public static class Conventions
    {
        public const int MIN_AMOUNT = 1;
    }
}
