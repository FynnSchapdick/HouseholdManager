using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using ShoppingService.Domain;

namespace ShoppingService.UnitTests;

public sealed class Test_ShoppingList_UpdateItem
{
    private readonly string _validShoppingListName = new Faker()
        .Random
        .String(
            ShoppingList.Conventions.NAME_MIN_LENGTH,
            ShoppingList.Conventions.NAME_MAX_LENGTH);

    [Fact]
    public void Should_ReturnTrue_WhenAmountIsUpdated()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        int initValue = 1;
        int updatedValue = 100;
        ShoppingList shoppingList = ShoppingList.CreateNew(_validShoppingListName);
        shoppingList.AddItem(productId, initValue);

        // Act
        bool updated = shoppingList.UpdateItem(productId, 100);
        
        // Assert
        using var scope = new AssertionScope();
        updated.Should().BeTrue("because the amount of product {0} was updated", productId);
        ShoppingListItem shoppingListItem = shoppingList.Items.Single(x => x.ProductId == productId);
        shoppingListItem.Amount.Should().Be(updatedValue, "because the amount should be updated to {0}", updatedValue);
    }
    
    [Fact]
    public void Should_ReturnFalse_WhenAmountIsNotUpdated()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        int initValue = 1;
        int updatedValue = 100;
        ShoppingList shoppingList = ShoppingList.CreateNew(_validShoppingListName);
        shoppingList.AddItem(productId, initValue);

        // Act
        bool updated = shoppingList.UpdateItem(Guid.NewGuid(), 100);
        
        // Assert
        using var scope = new AssertionScope();
        updated.Should().BeFalse("because the amount of product {0} was not updated", productId);
        ShoppingListItem shoppingListItem = shoppingList.Items.Single(x => x.ProductId == productId);
        shoppingListItem.Amount.Should().Be(initValue, "because the amount of {0} remains unchanged", initValue);
    }
}