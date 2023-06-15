using Bogus;
using FluentAssertions;
using HouseholdManager.Api.Domain.Shopping;
using ShoppingUnitTests.Assertions;
using Testing.Shared.Assertions.Assertions;

namespace ShoppingUnitTests;

public sealed class Test_ShoppingList_Create
{
    private readonly string _validShoppingListName = new Faker()
        .Random
        .String(
            ShoppingListAggregate.Conventions.NAME_MIN_LENGTH,
            ShoppingListAggregate.Conventions.NAME_MAX_LENGTH);

    private readonly string _tooShortShoppingListName = new Faker()
        .Random
        .String(
            ShoppingListAggregate.Conventions.NAME_MIN_LENGTH - 1);

    private readonly string _tooLongShoppingListName = new Faker()
        .Random
        .String(
            ShoppingListAggregate.Conventions.NAME_MAX_LENGTH + 1);

    [Fact]
    public void Should_NotThrowArgumentException_WhenNameIsValid()
    {
        var sut = () => ShoppingListAggregate.CreateNew(_validShoppingListName);
        sut.Should().NotThrow("because {0} is a valid name", _validShoppingListName)
            .Which.Should().NotHaveDefaultId("because a default id is not valid for a ShoppingList")
            .And.HaveName(_validShoppingListName)
            .And.BeEmpty();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsEmpty()
    {
        Action sut = () => ShoppingListAggregate.CreateNew(string.Empty);

        sut.Should().Throw<ArgumentException>("because the name should not be empty")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsWhitespace()
    {
        Action sut = () => ShoppingListAggregate.CreateNew("   ");

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
        Action sut = () => ShoppingListAggregate.CreateNew(_tooShortShoppingListName);
        sut.Should().Throw<ArgumentException>("because the name should not be too short")
            .WhichShouldHaveAMessage();
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenNameIsTooLong()
    {
        Action sut = () => ShoppingListAggregate.CreateNew(_tooLongShoppingListName);
        sut.Should().Throw<ArgumentException>("because the name should not be too long")
            .WhichShouldHaveAMessage();
    }
}
