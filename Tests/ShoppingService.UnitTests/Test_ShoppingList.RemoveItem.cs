using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using ShoppingService.Domain;

namespace ShoppingService.UnitTests;

public sealed class Test_ShoppingList_RemoveItem
{
    private readonly string _validShoppingListName = new Faker()
        .Random
        .String(
            ShoppingList.Conventions.NAME_MIN_LENGTH,
            ShoppingList.Conventions.NAME_MAX_LENGTH);

    
    [Fact]
    public void Should_ReturnTrue_WhenProductIsIncludedInItems()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        ShoppingList shoppingList = ShoppingList.CreateNew(_validShoppingListName);
        shoppingList.AddItem(productId, 10);

        // Act
        bool removed = shoppingList.RemoveItem(productId);
        
        // Assert
        using var scope = new AssertionScope();
        removed.Should().BeTrue($"because the product {productId} is included in the list", productId);
    }
    
    [Fact]
    public void Should_ReturnFalse_WhenProductNotIncludedInItems()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        ShoppingList shoppingList = ShoppingList.CreateNew(_validShoppingListName);

        // Act
        bool removed = shoppingList.RemoveItem(productId);
        
        // Assert
        using var scope = new AssertionScope();
        removed.Should().BeFalse($"because the product {productId} is not included in the list", productId);
    }
}