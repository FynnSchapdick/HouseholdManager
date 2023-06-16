using FluentAssertions;
using FluentAssertions.Execution;
using HouseholdManager.Domain.Product;
using ProductUnitTests.Assertions;
using ProductUnitTests.Data;
using Testing.Shared.Assertions.Assertions;

namespace ProductUnitTests;

public sealed class Test_Product_Create
{
    [Fact]
    public void Should_NotThrowArgumentException_WhenEanIsNull()
    {
        Action sut = () => ProductAggregate.CreateNew(Valid.ProductName);
        sut.Should().NotThrow("because products without ean are custom products");
    }

    [Fact]
    public void Should_NotThrowArgumentException_WhenEanIsEan8()
    {
        Action sut = () => ProductAggregate.CreateNew(Valid.ProductName, Valid.Ean8);
        sut.Should().NotThrow("because products with a valid ean 8 are ok");
    }

    [Fact]
    public void Should_NotThrowArgumentException_WhenEanIsEan13()
    {
        Action sut = () => ProductAggregate.CreateNew(Valid.ProductName, Valid.Ean13);
        sut.Should().NotThrow("because products with a valid ean 13 are ok");
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenEanIsEanInvalid()
    {
        Action sut = () => ProductAggregate.CreateNew(Valid.ProductName, Invalid.Ean);
        sut.Should().Throw<ArgumentException>("because {0} is not a valid ean", Invalid.Ean)
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenEanIsEmpty()
    {
        Action sut = () => ProductAggregate.CreateNew(Valid.ProductName, Invalid.EmptyEan);
        sut.Should().Throw<ArgumentException>("because ean may not be empty if it is given")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenEan8IsWhitespace()
    {
        Action sut = () => ProductAggregate.CreateNew(Valid.ProductName, Invalid.WhitespaceEan8);
        sut.Should().Throw<ArgumentException>("because ean may not be whitespace if it is given")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenEan13IsWhitespace()
    {
        Action sut = () => ProductAggregate.CreateNew(Valid.ProductName, Invalid.WhitespaceEan13);
        sut.Should().Throw<ArgumentException>("because ean may not be whitespace if it is given")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsNull()
    {
        Action sut = () => ProductAggregate.CreateNew(null!);
        sut.Should().Throw<ArgumentException>("because product names may not be null")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsEmpty()
    {
        Action sut = () => ProductAggregate.CreateNew(Invalid.EmptyProductName);
        sut.Should().Throw<ArgumentException>("because product names may not be empty")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsWhitespace()
    {
        Action sut = () => ProductAggregate.CreateNew(Invalid.WhitespaceProductName);
        sut.Should().Throw<ArgumentException>("because product names may not be whitespace")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_CreateProduct_WhenNameIsValid()
    {
        // Act
        var product = ProductAggregate.CreateNew(Valid.ProductName);

        // Assert
        using var scope = new AssertionScope();
        product.Should().NotHaveDefaultId("because product id may not be default")
            .And.HaveName(Valid.ProductName, "because product was instantiated with name {0}", Valid.ProductName);
    }

    [Fact]
    public void Should_CreateProduct_WhenNameAndEan8AreValid()
    {
        // Act
        var product = ProductAggregate.CreateNew(Valid.ProductName, Valid.Ean8);

        // Assert
        using var scope = new AssertionScope();
        product.Should().NotHaveDefaultId("because product id may not be default")
            .And.HaveName(Valid.ProductName, "because product was instantiated with name {0}", Valid.ProductName)
            .And.HaveEan(Valid.Ean8, "because product was instantiated with ean {0}", Valid.Ean8);
    }

    [Fact]
    public void Should_CreateProduct_WhenNameAndEan13AreValid()
    {
        // Act
        var product = ProductAggregate.CreateNew(Valid.ProductName, Valid.Ean13);

        // Assert
        using var scope = new AssertionScope();
        product.Should().NotHaveDefaultId("because product id may not be default")
            .And.HaveName(Valid.ProductName, "because product was instantiated with name {0}", Valid.ProductName)
            .And.HaveEan(Valid.Ean13, "because product was instantiated with ean {0}", Valid.Ean13);
    }
}
