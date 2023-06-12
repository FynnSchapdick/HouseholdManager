using FluentAssertions;
using ProductService.Domain;
using Testing.Shared.Assertions.Assertions;

namespace ProductService.UnitTests;

public sealed class Test_Product_Create
{
    [Fact]
    public void Should_NotThrowArgumentException_WhenEanIsNull()
    {
        Action sut = () => Product.CreateNew("Testname");
        sut.Should().NotThrow("because products without ean are custom products");
    }

    [Fact]
    public void Should_NotThrowArgumentException_WhenEanIsEan8()
    {
        Action sut = () => Product.CreateNew("Testname", "30034440");
        sut.Should().NotThrow("because products with a valid ean 8 are ok");
    }

    [Fact]
    public void Should_NotThrowArgumentException_WhenEanIsEan13()
    {
        Action sut = () => Product.CreateNew("Testname", "4102380501330");
        sut.Should().NotThrow("because products with a valid ean 13 are ok");
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenEanIsEanInvalid()
    {
        var invalidEan = "41023805013308";

        Action sut = () => Product.CreateNew("Testname", invalidEan);
        sut.Should().Throw<ArgumentException>("because {0} is not a valid ean", invalidEan)
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenEanIsEmpty()
    {
        Action sut = () => Product.CreateNew("Testname", "");
        sut.Should().Throw<ArgumentException>("because ean may not be empty if it is given")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenEanIsWhitespace()
    {
        Action sut = () => Product.CreateNew("Testname", "   ");
        sut.Should().Throw<ArgumentException>("because ean may not be whitespace if it is given")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsNull()
    {
        Action sut = () => Product.CreateNew(null!);
        sut.Should().Throw<ArgumentException>("because product names may not be null")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsEmpty()
    {
        Action sut = () => Product.CreateNew("");
        sut.Should().Throw<ArgumentException>("because product names may not be empty")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsWhitespace()
    {
        Action sut = () => Product.CreateNew("   ");
        sut.Should().Throw<ArgumentException>("because product names may not be whitespace")
            .WhichShouldHaveAMessage();
    }
}
