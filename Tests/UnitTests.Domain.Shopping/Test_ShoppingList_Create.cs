using FluentAssertions;
using HouseholdManager.Domain.Shopping;
using UnitTests.Domain.Shopping.Assertions;
using Testing.Shared.Assertions.Assertions;
using UnitTests.Domain.Shopping.Data;

namespace UnitTests.Domain.Shopping;

public sealed class Test_ShoppingList_Create
{


    [Fact]
    public void Should_NotThrowArgumentException_WhenNameIsValid()
    {
        Func<ShoppingListAggregate> sut = () => ShoppingListAggregate.CreateNew(Valid.ShoppingListName);
        sut.Should().NotThrow("because {0} is a valid name", Valid.ShoppingListName)
            .Which.Should().NotHaveDefaultId("because a default id is not valid for a ShoppingList")
            .And.HaveName(Valid.ShoppingListName)
            .And.BeEmpty();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsEmpty()
    {
        Action sut = () => ShoppingListAggregate.CreateNew(Invalid.EmptyShoppingListName);

        sut.Should().Throw<ArgumentException>("because the name should not be empty")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsWhitespace()
    {
        Action sut = () => ShoppingListAggregate.CreateNew(Invalid.WhitespaceShoppingListName);

        sut.Should().Throw<ArgumentException>("because the name should not be whitespace")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentNullException_WhenNameIsNull()
    {
        Action sut = () => ShoppingListAggregate.CreateNew(null!);
        sut.Should().Throw<ArgumentException>("because the name may not be null")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsTooShort()
    {
        Action sut = () => ShoppingListAggregate.CreateNew(Invalid.ShortShoppingListName);
        sut.Should().Throw<ArgumentException>("because the name should not be too short")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsTooLong()
    {
        Action sut = () => ShoppingListAggregate.CreateNew(Invalid.LongShoppingListName);
        sut.Should().Throw<ArgumentException>("because the name should not be too long")
            .WhichShouldHaveAMessage();
    }
}
