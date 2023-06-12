﻿using FluentAssertions;
using FluentAssertions.Primitives;
using ShoppingService.Domain;

namespace ShoppingService.UnitTests.Assertions;

public sealed class ShoppingListAssertions : ReferenceTypeAssertions<ShoppingList, ShoppingListAssertions>
{
    public ShoppingListAssertions(ShoppingList shoppingList) : base(shoppingList){}
    protected override string Identifier => "shoppinglist";

    public AndConstraint<ShoppingListAssertions> HaveName(string name, string because = "", params object[] becauseArgs)
    {
        Subject.Name.Should().Be(name, because, becauseArgs);
        return new AndConstraint<ShoppingListAssertions>(this);
    }
    
    public AndConstraint<ShoppingListAssertions> NotHaveDefaultId(string because = "", params object[] becauseArgs)
    {
        Subject.Id.Should().NotBeEmpty(because, becauseArgs);
        return new AndConstraint<ShoppingListAssertions>(this);
    }
    
    public AndConstraint<ShoppingListAssertions> BeEmpty(string because = "", params object[] becauseArgs)
    {
        Subject.Items.Should().BeEmpty(because, becauseArgs);
        return new AndConstraint<ShoppingListAssertions>(this);
    }
}

public static class ShoppingListAssertionsExtensions
{
    public static ShoppingListAssertions Should(this ShoppingList instance) => new(instance);
}