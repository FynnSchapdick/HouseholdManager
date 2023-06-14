using FluentAssertions;
using HouseholdManager.Api.Domain;

namespace ShoppingUnitTests.Assertions;

public static class AssertionsExtensions
{
    public static ShoppingListAssertions Should(this ShoppingList instance) => new(instance);
    public static AggregateAssertions<ShoppingList> AsAggregateShould(this ShoppingList instance) => new(instance);
    public static ShoppingListItemAssertions Should(this ShoppingListItem instance) => new(instance);

    public static AndConstraint<T> Which<T, TSubject>(this AndWhichConstraint<T, TSubject> constraint, Func<TSubject, object> assertion)
    {
        assertion(constraint.Which);
        return new AndConstraint<T>(constraint.And);
    }
}
