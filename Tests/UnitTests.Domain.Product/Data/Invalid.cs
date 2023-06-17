using Bogus;
using HouseholdManager.Domain.Product;

namespace UnitTests.Domain.Product.Data;

public static class Invalid
{
    public static readonly string EmptyProductName = string.Empty;
    public static readonly string WhitespaceEan8 = new(' ', 8);
    public static readonly string WhitespaceEan13 = new(' ', 13);
    public static readonly string EmptyEan = string.Empty;
    public static readonly string Ean = new Faker().Random.String(255);
    public static readonly string WhitespaceProductName = new(' ', ProductAggregate.Conventions.NAME_MIN_LENGTH);
}
