using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using HouseholdManager.Api.Domain.Shopping;

namespace ShoppingUnitTests;

public sealed class Test_ShoppingList_RemoveItem
{
    private readonly string _validShoppingListName = new Faker()
        .Random
        .String(
            ShoppingListAggregate.Conventions.NAME_MIN_LENGTH,
            ShoppingListAggregate.Conventions.NAME_MAX_LENGTH);


    [Fact]
    public void Should_ReturnTrue_WhenProductIsIncludedInItems()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var shoppingList = ShoppingListAggregate.CreateNew(_validShoppingListName);
        shoppingList.AddItem(productId, 10);

        // Act
        bool removed = shoppingList.RemoveItem(productId);

        // Assert
        using var scope = new AssertionScope();
        removed.Should().BeTrue("because the product {0} is included in the list", productId);
    }

    [Fact]
    public void Should_ReturnFalse_WhenProductNotIncludedInItems()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var shoppingList = ShoppingListAggregate.CreateNew(_validShoppingListName);

        // Act
        bool removed = shoppingList.RemoveItem(productId);

        // Assert
        using var scope = new AssertionScope();
        removed.Should().BeFalse("because the product {0} is not included in the list", productId);
    }
}
