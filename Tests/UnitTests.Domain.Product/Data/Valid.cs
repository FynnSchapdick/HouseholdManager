using Bogus;
using HouseholdManager.Domain.Product;

namespace UnitTests.Domain.Product.Data;

public static class Valid
{
    public static readonly string ProductName = new Faker()
        .Random
        .String(
            ProductAggregate.Conventions.NAME_MIN_LENGTH,
            ProductAggregate.Conventions.NAME_MAX_LENGTH);

    public static readonly string Ean8 = new Faker().Commerce.Ean8();
    public static readonly string Ean13 = new Faker().Commerce.Ean13();
}