using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using ShoppingService.Domain;
using ShoppingService.UnitTests.Assertions;

namespace ShoppingService.UnitTests;

public sealed class Test_ShoppingList_UpdateItem
{
    private readonly string _validShoppingListName = new Faker()
        .Random
        .String(
            ShoppingList.Conventions.NAME_MIN_LENGTH,
            ShoppingList.Conventions.NAME_MAX_LENGTH);

    [Theory]
    [InlineData(1, 100), InlineData(100, 1)]
    public void Should_ReturnTrue_WhenAmountIsUpdated(int initialAmount, int targetAmount)
    {
        // Arrange
        var shoppingList = ShoppingList.CreateNew(_validShoppingListName);

        var productId = Guid.NewGuid();
        shoppingList.AddItem(productId, initialAmount);

        // Act
        bool updated = shoppingList.UpdateItem(productId, targetAmount);

        // Assert
        using var scope = new AssertionScope();
        updated.Should().BeTrue("because the amount of product {0} was updated", productId);
        shoppingList.Items.Should().ContainSingle("because only a single item was added in setup")
            .Which.Should().BeForProductId(productId, "because that is the product id of the item added during setup")
            .And.HaveAmount(targetAmount, "because the update set the amount to {0}", targetAmount);
    }

    [Fact]
    public void Should_ReturnFalse_WhenShoppingListDoesNotContainTheItem()
    {
        // Arrange
        var shoppingList = ShoppingList.CreateNew(_validShoppingListName);

        Guid targetItemProductId = Guid.Parse("274D4C9D-12C0-4543-9996-CDDBBCF1C0D7");
        const int targetAmount = 100;

        // Act
        bool updated = shoppingList.UpdateItem(targetItemProductId, targetAmount);

        // Assert
        using var scope = new AssertionScope();
        shoppingList.Items.Should().BeEmpty("because no items were added");
        updated.Should().BeFalse("because the list does not contain an item with product id {0}", targetItemProductId);
    }
}
