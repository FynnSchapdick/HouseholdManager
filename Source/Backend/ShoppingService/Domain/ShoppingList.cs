using System.Collections.Immutable;
using Throw;

namespace ShoppingService.Domain;

public record ShoppingList
{
    public const int NameMaxLength = 75;
    public const int NameMinLength = 5;
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public HashSet<ShoppingItem> ShoppingItems { get; init; } = new();
    public static ShoppingList CreateNew(string name)
    {
        return new ShoppingList
        {
            Id = Guid.NewGuid(),
            Name = name
                .Throw()
                .IfShorterThan(NameMinLength)
                .IfLongerThan(NameMaxLength)
        };
    }
}