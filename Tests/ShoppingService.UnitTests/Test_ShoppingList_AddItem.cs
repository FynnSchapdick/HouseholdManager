using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using ShoppingService.Domain;
using ShoppingService.UnitTests.Assertions;

namespace ShoppingService.UnitTests;

public sealed class Test_ShoppingList_AddItem
{
    private readonly string _validShoppingListName = new Faker()
        .Random
        .String(
            ShoppingList.Conventions.NAME_MIN_LENGTH,
            ShoppingList.Conventions.NAME_MAX_LENGTH);

    [Theory]
    [InlineData(0), InlineData(-1)]
    public void Should_ThrowArgumentOutOfRangeException_WhenAmountIsLessThanOne(int amount)
    {
        // Arrange
        var shoppingList = ShoppingList.CreateNew(_validShoppingListName);

        // Act + Assert
        shoppingList.Invoking(x => x.AddItem(Guid.NewGuid(), amount))
            .Should().Throw<ArgumentOutOfRangeException>("because buying 0 or less of something does not work in real life");
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenProductIdIsEmpty()
    {
        // Arrange
        var shoppingList = ShoppingList.CreateNew(_validShoppingListName);

        // Act + Assert
        shoppingList.Invoking(x => x.AddItem(Guid.Empty, 1))
            .Should().Throw<ArgumentException>("because a valid product id is required to create an item");
    }

    [Fact]
    public void Should_AddNewShoppingListItem_WhenProductNotIncludedInItemsYet()
    {
        // Arrange
        var productId = Guid.NewGuid();
        const int amount = 10;
        var shoppingList = ShoppingList.CreateNew(_validShoppingListName);

        // Act
        shoppingList.AddItem(productId, amount);

        // Assert
        using var scope = new AssertionScope();
        shoppingList
            .Items.Should().ContainSingle("because only a single item was added")
            .Which.Should().HaveAmount(amount, "because that is the amount of the added item")
            .And.BeForProductId(productId, "because that is the product id of the added item");
    }

    [Fact]
    public void Should_IncreaseAmount_WhenProductIsAlreadyIncludedInItems()
    {
        // Arrange
        int[] amounts = { 1, 99 };
        var productId = Guid.NewGuid();
        var shoppingList = ShoppingList.CreateNew(_validShoppingListName);
        shoppingList.AddItem(productId, amounts[0]);

        // Act
        shoppingList.AddItem(productId, amounts[1]);

        // Assert
        using var scope = new AssertionScope();
        shoppingList
            .Items.Should().ContainSingle("because adding the same item twice should combine them")
            .Which.Should().HaveAmount(amounts.Sum(), "because that is the sum of the amount of both items")
            .And.BeForProductId(productId, "because that is the product id of the targeted item");
    }
}
