using ProductService.Domain;

namespace ProductService.UnitTests;

public class ProductTests
{
    [Fact]
    public void CreateProduct_ShouldNotThrowArgumentException_WhenEanIsNull()
    {
        Exception? exception = Record.Exception(() => Product.CreateNew("Testname"));
        Assert.Null(exception);
    }
    
    [Fact]
    public void CreateProduct_ShouldThrowArgumentException_WhenEanIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => Product.CreateNew("Testname", ""));
    }
}