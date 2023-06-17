using FluentAssertions;
using FluentAssertions.Execution;
using HouseholdManager.Domain.Shopping;
using HouseholdManager.Domain.Shopping.ValueObjects;
using UnitTests.Domain.Shopping.Assertions;
using UnitTests.Domain.Shopping.Data;

namespace UnitTests.Domain.Shopping;

public sealed class Test_ShoppingList_UpdateItem
{
    [Theory]
    [InlineData(1, 100), InlineData(100, 1)]
    public void Should_ReturnTrue_WhenAmountIsUpdated(int initialAmount, int targetAmount)
    {
        // Arrange
        ShoppingListAggregate shoppingList = Valid.ShoppingListWithSingleItem(initialAmount);
        ShoppingListItem item = shoppingList.Items.First();

        // Act
        bool updated = shoppingList.UpdateItem(item.ProductId, targetAmount);

        // Assert
        using var scope = new AssertionScope();
        updated.Should().BeTrue("because the amount of product {0} was updated", item.ProductId);
        shoppingList.Items.Should().ContainSingle("because only a single item was added in setup")
            .Which.Should().BeForProductId(item.ProductId, "because that is the product id of the item added during setup")
            .And.HaveAmount(targetAmount, "because the update set the amount to {0}", targetAmount);
    }

    [Fact]
    public void Should_ReturnFalse_WhenShoppingListDoesNotContainTheItem()
    {
        // Arrange
        ShoppingListAggregate shoppingList = Valid.EmptyShoppingList();
        var targetItemProductId = Guid.NewGuid();
        const int targetAmount = 100;

        // Act
        bool updated = shoppingList.UpdateItem(targetItemProductId, targetAmount);

        // Assert
        using var scope = new AssertionScope();
        shoppingList.Items.Should().BeEmpty("because no items were added");
        updated.Should().BeFalse("because the list does not contain an item with product id {0}", targetItemProductId);
    }
}