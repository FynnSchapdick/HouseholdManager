using Bogus;
using HouseholdManager.Domain.Shopping;

namespace ShoppingUnitTests.Data;

public static class Invalid
{
    public static readonly string EmptyShoppingListName = string.Empty;
    public static readonly string WhitespaceShoppingListName = new(' ', ShoppingListAggregate.Conventions.NAME_MIN_LENGTH);

    public static readonly string ShortShoppingListName = new Faker()
        .Random
        .String(ShoppingListAggregate.Conventions.NAME_MIN_LENGTH - 1);

    public static readonly string LongShoppingListName = new Faker()
        .Random
        .String(ShoppingListAggregate.Conventions.NAME_MAX_LENGTH + 1);
}
