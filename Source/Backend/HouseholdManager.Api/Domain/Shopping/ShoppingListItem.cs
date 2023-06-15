using Throw;

namespace HouseholdManager.Api.Domain.Shopping;

public sealed record ShoppingListItem
{
    public Guid ShoppingListId { get; private init; }
    public Guid ProductId { get; private init; }
    public int Amount { get; private set; } = Conventions.MIN_AMOUNT;

    public ProductInfo? ProductInfo { get; private set; }

    public static ShoppingListItem CreateNew(Guid shoppingListId, Guid productId, int amount)
    {
        return new ShoppingListItem
        {
            ShoppingListId = shoppingListId.Throw().IfDefault(),
            ProductId = productId.Throw().IfDefault(),
            Amount = amount.Throw().IfLessThan(Conventions.MIN_AMOUNT)
        };
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

    public sealed class Conventions
    {
        public const int MIN_AMOUNT = 1;
    }
}

public sealed record ProductInfo(string ProductName);
