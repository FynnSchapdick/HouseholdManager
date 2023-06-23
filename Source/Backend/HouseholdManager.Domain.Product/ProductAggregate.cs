using HouseholdManager.Domain.Product.Extensions;
using Throw;

namespace HouseholdManager.Domain.Product;

public sealed record ProductAggregate
{
    public Guid ProductId { get; }
    public string Name { get; private set; }
    public string? Ean { get; private set; }

#pragma warning disable CS8618
    private ProductAggregate()
    {
        /*Ef*/
    }
#pragma warning restore CS8618

    internal ProductAggregate(string name, string? ean = null)
    {
        ProductId = Guid.NewGuid();
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

    public override string ToString()
    {
        return $"Product {ProductId} '{Name}'";
    }

    public static class Conventions
    {
        public const int NAME_MAX_LENGTH = 100;
        public const int NAME_MIN_LENGTH = 2;
    }
}
