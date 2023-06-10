using ProductService.Extensions;
using Throw;

namespace ProductService.Domain;

public sealed record Product
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Ean { get; init; }

    public static Product CreateNew(string name, string? ean = null)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name
                .Throw()
                .IfEmpty()
                .IfShorterThan(Conventions.NameMinLength)
                .IfLongerThan(Conventions.NameMaxLength),
            Ean = ean?
                .Throw()
                .IfNotEan8Or13()
        };
    }

    public sealed class Conventions
    {
        public const int NameMaxLength = 100;
        public const int NameMinLength = 2;
    }
}