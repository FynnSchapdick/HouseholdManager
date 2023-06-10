using System.Collections.Immutable;
using Throw;

namespace ShoppingService.Domain;

public record ShoppingList
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public HashSet<ShoppingListItem> Items { get; init; } = new();
    public static ShoppingList CreateNew(string name)
    {
        return new ShoppingList
        {
            Id = Guid.NewGuid(),
            Name = name.Throw()
                .IfShorterThan(Conventions.NAME_MIN_LENGTH)
                .IfLongerThan(Conventions.NAME_MAX_LENGTH)
        };
    }

    public sealed class Conventions
    {
        public const int NAME_MAX_LENGTH = 100;
        public const int NAME_MIN_LENGTH = 5;
    }
}
