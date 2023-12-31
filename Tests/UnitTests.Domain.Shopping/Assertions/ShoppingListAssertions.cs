﻿using FluentAssertions;
using FluentAssertions.Primitives;
using HouseholdManager.Domain.Shopping;

namespace UnitTests.Domain.Shopping.Assertions;

public sealed class ShoppingListAssertions : ReferenceTypeAssertions<ShoppingListAggregate, ShoppingListAssertions>
{
    public ShoppingListAssertions(ShoppingListAggregate shoppingList) : base(shoppingList)
    {
    }

    protected override string Identifier => "shoppinglist";

    public AndConstraint<ShoppingListAssertions> HaveName(string name, string because = "", params object[] becauseArgs)
    {
        Subject.Name.Should().Be(name, because, becauseArgs);
        return new AndConstraint<ShoppingListAssertions>(this);
    }

    public AndConstraint<ShoppingListAssertions> NotHaveDefaultId(string because = "", params object[] becauseArgs)
    {
        Subject.ShoppingListId.Should().NotBeEmpty(because, becauseArgs);
        return new AndConstraint<ShoppingListAssertions>(this);
    }

    public AndConstraint<ShoppingListAssertions> BeEmpty(string because = "", params object[] becauseArgs)
    {
        Subject.Items.Should().BeEmpty(because, becauseArgs);
        return new AndConstraint<ShoppingListAssertions>(this);
    }
}