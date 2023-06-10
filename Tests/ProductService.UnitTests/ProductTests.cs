using ProductService.Domain;

namespace ProductService.UnitTests;

public sealed class ProductTests
{
    [Fact]
    public void CreateProduct_ShouldNotThrowArgumentException_WhenEanIsNull()
    {
        Exception? exception = Record.Exception(() => Product.CreateNew("Testname"));
        Assert.Null(exception);
    }
    
    [Fact]
    public void CreateProduct_ShouldNotThrowArgumentException_WhenEanIsEan8()
    {
        Exception? exception = Record.Exception(() => Product.CreateNew("Testname", "30034440"));
        Assert.Null(exception);
    }
    
    [Fact]
    public void CreateProduct_ShouldNotThrowArgumentException_WhenEanIsEan13()
    {
        Exception? exception = Record.Exception(() => Product.CreateNew("Testname", "4102380501330"));
        Assert.Null(exception);
    }
    
    [Fact]
    public void CreateProduct_ShouldNotThrowArgumentException_WhenEanIsEanInvalid()
    {
        Assert.Throws<ArgumentException>(() => Product.CreateNew("Testname", "41023805013308"));
    }
    
    [Fact]
    public void CreateProduct_ShouldThrowArgumentException_WhenEanIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => Product.CreateNew("Testname", ""));
    }
    
    [Fact]
    public void CreateProduct_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => Product.CreateNew(""));
    }
}