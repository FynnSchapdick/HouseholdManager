using Bogus;
using HouseholdManager.Domain.Shopping;
using HouseholdManager.Domain.Shopping.ValueObjects;

namespace UnitTests.Domain.Shopping.Data;

public static class Valid
{
    public static readonly string ShoppingListName = new Faker()
        .Random
        .String(
            ShoppingListAggregate.Conventions.NAME_MIN_LENGTH,
            ShoppingListAggregate.Conventions.NAME_MAX_LENGTH);

    public static ShoppingListAggregate EmptyShoppingList()
    {
        return new ShoppingListAggregate(Guid.NewGuid(), ShoppingListName, new HashSet<ShoppingListItem>());
    }

    public static ShoppingListAggregate ShoppingListWithSingleItem(int amount = ShoppingListItem.Conventions.MIN_AMOUNT)
    {
        var id = Guid.NewGuid();
        var shoppingList = new ShoppingListAggregate(id, ShoppingListName, new HashSet<ShoppingListItem>
        {
            new(id, Guid.NewGuid(), amount)
        });
        return shoppingList;
    }

    public static ShoppingListAggregate ShoppingListWithNItems(int itemCount)
    {
        var id = Guid.NewGuid();
        IEnumerable<ShoppingListItem> items = Enumerable.Range(0, itemCount)
            .Select(_ => new ShoppingListItem(id, Guid.NewGuid(), Random.Shared.Next(ShoppingListItem.Conventions.MIN_AMOUNT, ShoppingListItem.Conventions.MIN_AMOUNT + 1000)));

        var shoppingList = new ShoppingListAggregate(id, ShoppingListName, items.ToHashSet());

        return shoppingList;
    }
}
