using FluentAssertions;
using FluentAssertions.Primitives;
using HouseholdManager.Api.Domain;

namespace ShoppingUnitTests.Assertions;

public sealed class ShoppingListItemAssertions : ReferenceTypeAssertions<ShoppingListItem, ShoppingListItemAssertions>
{
    public ShoppingListItemAssertions(ShoppingListItem item) : base(item){}
    protected override string Identifier => "shoppinglist_item";

    public AndConstraint<ShoppingListItemAssertions> HaveAmount(int amount, string because = "", params object[] becauseArgs)
    {
        Subject.Amount.Should().Be(amount, because, becauseArgs);
        return new AndConstraint<ShoppingListItemAssertions>(this);
    }

    public AndConstraint<ShoppingListItemAssertions> BeForProductId(Guid productId, string because = "", params object[] becauseArgs)
    {
        Subject.ProductId.Should().Be(productId, because, becauseArgs);
        return new AndConstraint<ShoppingListItemAssertions>(this);
    }
    
    public AndConstraint<ShoppingListItemAssertions> BeForShoppingListId(Guid shoppingListId, string because = "", params object[] becauseArgs)
    {
        Subject.ShoppingListId.Should().Be(shoppingListId, because, becauseArgs);
        return new AndConstraint<ShoppingListItemAssertions>(this);
    }
}

public static class ShoppingListItemAssertionsExtensions
{
    public static ShoppingListItemAssertions Should(this ShoppingListItem instance) => new(instance);
}
