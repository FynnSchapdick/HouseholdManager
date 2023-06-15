using FluentAssertions;
using HouseholdManager.Api.Domain.Shopping;
using HouseholdManager.Api.Domain.Shopping.ValueObjects;

namespace ShoppingUnitTests.Assertions;

public static class AssertionsExtensions
{
    public static ShoppingListAssertions Should(this ShoppingListAggregate instance) => new(instance);
    public static AggregateAssertions<ShoppingListAggregate> AsAggregateShould(this ShoppingListAggregate instance) => new(instance);
    public static ShoppingListItemAssertions Should(this ShoppingListItem instance) => new(instance);

    public static AndConstraint<T> Which<T, TSubject>(this AndWhichConstraint<T, TSubject> constraint, Func<TSubject, object> assertion)
    {
        assertion(constraint.Which);
        return new AndConstraint<T>(constraint.And);
    }
}
