using HouseholdManager.Domain.Product.Extensions;
using Throw;

namespace HouseholdManager.Domain.Product;

public sealed record ProductAggregate
{
    public  Guid Id { get; }
    public  string Name { get; private set; }
    public string? Ean { get; private set; }

    internal ProductAggregate(string name, string? ean = null)
    {
        Id = Guid.NewGuid();
        Name = name
            .ThrowIfNull()
            .IfEmpty()
            .IfWhiteSpace()
            .IfShorterThan(Conventions.NAME_MIN_LENGTH)
            .IfLongerThan(Conventions.NAME_MAX_LENGTH);
        Ean = ean?.Throw().IfEmpty().IfWhiteSpace().IfNotEan8Or13();
    }

    public static ProductAggregate CreateNew(string name, string? ean = null)
    {
        return new ProductAggregate(name, ean);
    }

    public static class Conventions
    {
        public const int NAME_MAX_LENGTH = 100;
        public const int NAME_MIN_LENGTH = 2;
    }
}
