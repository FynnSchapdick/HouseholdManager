using HouseholdManager.Api.Extensions;
using Throw;

namespace HouseholdManager.Api.Domain.Product;

public sealed record ProductAggregate
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Ean { get; init; }
    public static ProductAggregate CreateNew(string name, string? ean = null)
    {
        return new ProductAggregate
        {
            Id = Guid.NewGuid(),
            Name = name
                .ThrowIfNull()
                .IfEmpty()
                .IfWhiteSpace()
                .IfShorterThan(Conventions.NAME_MIN_LENGTH)
                .IfLongerThan(Conventions.NAME_MAX_LENGTH),
            Ean = ean?.Throw().IfNotEan8Or13()
        };
    }

    public sealed class Conventions
    {
        public const int NAME_MAX_LENGTH = 100;
        public const int NAME_MIN_LENGTH = 2;
    }
}
