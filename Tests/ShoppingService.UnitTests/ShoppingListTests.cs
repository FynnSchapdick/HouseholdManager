using ShoppingService.Domain;

namespace ShoppingService.UnitTests;

public sealed class ShoppingListTests
{
    [Fact]
    public void CreateShoppingList_ShouldNotThrowArgumentException()
    {
        Exception? exception = Record.Exception(() => ShoppingList.CreateNew("Testname"));
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
        Assert.Throws<ArgumentNullException>(() => ShoppingList.CreateNew(string.Empty));
    }

    [Theory]
    [InlineData("A")]
    [InlineData("ABCD")]
    public void CreateShoppingList_ShouldThrowArgumentException_WhenNameIsTooShort(string name)
    {
        Assert.Throws<ArgumentException>(() => ShoppingList.CreateNew(name));
    }
}