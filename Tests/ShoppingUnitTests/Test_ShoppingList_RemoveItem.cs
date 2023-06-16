using FluentAssertions;
using FluentAssertions.Execution;
using HouseholdManager.Domain.Shopping;
using HouseholdManager.Domain.Shopping.ValueObjects;
using ShoppingUnitTests.Data;

namespace ShoppingUnitTests;

public sealed class Test_ShoppingList_RemoveItem
{

    [Fact]
    public void Should_ReturnTrue_WhenProductIsIncludedInItems()
    {
        // Arrange
        ShoppingListAggregate shoppingList = Valid.ShoppingListWithSingleItem();
        ShoppingListItem item = shoppingList.Items.First();

        // Act
        bool removed = shoppingList.RemoveItem(item.ProductId);

        // Assert
        using var scope = new AssertionScope();
        removed.Should().BeTrue("because the product {0} is included in the list", item.ProductId);
        shoppingList.Items.Should().BeEmpty("because the only item was just removed");
    }

    [Fact]
    public void Should_ReturnFalse_WhenProductNotIncludedInItems()
    {
        // Arrange
        var productId = Guid.NewGuid();
        ShoppingListAggregate shoppingList = Valid.EmptyShoppingList();

        // Act
        bool removed = shoppingList.RemoveItem(productId);

        // Assert
        using var scope = new AssertionScope();
        removed.Should().BeFalse("because the product {0} is not included in the list", productId);
    }
}
