using Bogus;
using ShoppingService.Domain;

namespace ShoppingService.UnitTests;

public sealed class ShoppingListTests
{
    private readonly string _validShoppingListName = new Faker()
        .Random
        .String(
            ShoppingList.Conventions.NAME_MIN_LENGTH,
            ShoppingList.Conventions.NAME_MAX_LENGTH);
    
    private readonly string _tooShortShoppingListName = new Faker()
        .Random
        .String(
            ShoppingList.Conventions.NAME_MIN_LENGTH - 1);
    
    private readonly string _tooLongShoppingListName = new Faker()
        .Random
        .String(
            ShoppingList.Conventions.NAME_MAX_LENGTH + 1);

    [Fact]
    public void CreateShoppingList_ShouldNotThrowArgumentException_WhenNameIsValid()
    {
        Exception? exception = Record.Exception(() => ShoppingList.CreateNew(_validShoppingListName));
        Assert.Null(exception);
    }
    
    [Fact]
    public void CreateShoppingList_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => ShoppingList.CreateNew(string.Empty));
    }
    
    [Fact]
    public void CreateShoppingList_ShouldThrowArgumentNullException_WhenNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ShoppingList.CreateNew(null));
    }

    [Fact]
    public void CreateShoppingList_ShouldThrowArgumentException_WhenNameIsTooShort()
    {
        Assert.Throws<ArgumentException>(() => ShoppingList.CreateNew(_tooShortShoppingListName));
    }
    
    [Fact]
    public void CreateShoppingList_ShouldThrowArgumentException_WhenNameIsTooLong()
    {
        Assert.Throws<ArgumentException>(() => ShoppingList.CreateNew(_tooLongShoppingListName));
    }

    [Fact]
    public void AddShoppingListItem_ShouldThrowArgumentOutOfRangeException_WhenAmountIsSmallerOne()
    {
        // Arrange
        ShoppingList shoppingList = ShoppingList.CreateNew(_validShoppingListName);
        
        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => shoppingList.AddItem(Guid.NewGuid(),  new Faker().Random.Int(-999, 0)));
    }
    
    [Fact]
    public void AddShoppingListItem_ShouldThrowArgumentException_WhenProductIdIsEmpty()
    {
        // Arrange
        ShoppingList shoppingList = ShoppingList.CreateNew(_validShoppingListName);
        
        // Act + Assert
        Assert.Throws<ArgumentException>(() => shoppingList.AddItem(Guid.Empty, 1));
    }

    [Fact]
    public void AddShoppingListItem_ShouldAddNewShoppingListItem_WhenProductNotIncludedInItemsYet()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        int amount = 10;
        ShoppingList shoppingList = ShoppingList.CreateNew(_validShoppingListName);
        
        // Act
        shoppingList.AddItem(productId, amount);

        // Assert
        Assert.Single(shoppingList.Items);
        ShoppingListItem shoppingListItem = shoppingList.Items.Single();
        Assert.Equal(amount, shoppingListItem.Amount);
        Assert.Equal(productId, shoppingListItem.ProductId);
        Assert.Equal(shoppingList.Id,shoppingListItem.ShoppingListId);
        Assert.Equal(_validShoppingListName, shoppingList.Name);
    }
    
    [Fact]
    public void AddShoppingListItem_ShouldIncreaseAmount_WhenProductIsAlreadyIncludedInItems()
    {
        // Arrange
        int[] amounts = {1, 99};
        Guid productId = Guid.NewGuid();
        ShoppingList shoppingList = ShoppingList.CreateNew(_validShoppingListName);
        shoppingList.AddItem(productId, amounts[0]);
        
        // Act
        shoppingList.AddItem(productId, amounts[1]);
        
        // Assert
        Assert.Single(shoppingList.Items);
        ShoppingListItem shoppingListItem = shoppingList.Items.Single();
        Assert.Equal(amounts.Sum(), shoppingListItem.Amount);
        Assert.Equal(productId, shoppingListItem.ProductId);
        Assert.Equal(shoppingList.Id,shoppingListItem.ShoppingListId);
        Assert.Equal(_validShoppingListName, shoppingList.Name);
    }
}