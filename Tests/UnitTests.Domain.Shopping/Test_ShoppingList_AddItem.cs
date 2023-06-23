using FluentAssertions;
using FluentAssertions.Execution;
using HouseholdManager.Domain.Shopping;
using HouseholdManager.Domain.Shopping.Events;
using HouseholdManager.Domain.Shopping.ValueObjects;
using UnitTests.Domain.Shopping.Assertions;
using Testing.Shared.Assertions.Assertions;
using UnitTests.Domain.Shopping.Data;

namespace UnitTests.Domain.Shopping;

public sealed class Test_ShoppingList_AddItem
{
    [Theory]
    [InlineData(0), InlineData(-1)]
    public void Should_ThrowArgumentOutOfRangeException_WhenAmountIsLessThanOne(int amount)
    {
        // Arrange
        ShoppingListAggregate shoppingList = Valid.EmptyShoppingList();

        // Act + Assert
        shoppingList.Invoking(x => x.AddItem(Guid.NewGuid(), amount))
            .Should().Throw<ArgumentOutOfRangeException>("because buying 0 or less of something does not work in real life")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenProductIdIsEmpty()
    {
        // Arrange
        ShoppingListAggregate shoppingList = Valid.EmptyShoppingList();

        // Act + Assert
        shoppingList.Invoking(x => x.AddItem(Guid.Empty, 1))
            .Should().Throw<ArgumentException>("because a valid product id is required to create an item")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_AddNewShoppingListItem_WhenProductNotIncludedInItemsYet()
    {
        // Arrange
        var productId = Guid.NewGuid();
        const int amount = 10;
        ShoppingListAggregate shoppingList = Valid.EmptyShoppingList();

        // Act
        shoppingList.AddItem(productId, amount);

        // Assert
        using var scope = new AssertionScope();

        shoppingList
            .AsAggregateShould().ContainSingleEvent<ShoppingListItemAddedEvent>("because only that event documents what actually happened")
            .Which(@event => @event.Should().BeEquivalentTo(new ShoppingListItemAddedEvent
                {
                    Amount = amount,
                    ProductId = productId,
                    ShoppingListId = shoppingList.ShoppingListId
                }, opt => opt.Excluding(x => x.Timestamp),
                "because that is the expected data"))
            .And.Items.Should().ContainSingle("because only a single item was added")
            .Which.Should().BeForShoppingListId(shoppingList.ShoppingListId, "because that is the id of the shoppingList")
            .And.HaveAmount(amount, "because that is the amount of the added item")
            .And.BeForProductId(productId, "because that is the product id of the added item");
    }

    [Theory]
    [InlineData(10, 10), InlineData(1, 100)]
    public void Should_IncreaseAmount_WhenProductIsAlreadyIncludedInItems(int initialAmount, int additionalAmount)
    {
        // Arrange
        ShoppingListAggregate shoppingList = Valid.ShoppingListWithSingleItem(initialAmount);
        ShoppingListItem item = shoppingList.Items.First();

        // Act
        shoppingList.AddItem(item.ProductId, additionalAmount);

        // Assert
        using var scope = new AssertionScope();
        shoppingList
            .AsAggregateShould().NotContainEvents()
            .And.Items.Should().ContainSingle("because adding the same item twice should combine them")
            .Which.Should().HaveAmount(initialAmount + additionalAmount, "because that is the sum of the amount of both operations")
            .And.BeForProductId(item.ProductId, "because that is the product id of the targeted item");
    }
}
