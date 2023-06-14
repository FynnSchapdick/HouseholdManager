using Throw;

namespace HouseholdManager.Api.Domain;

public record ShoppingList : Aggregate
{
    public Guid Id { get; private init; }
    public string Name { get; private init; }

    private readonly HashSet<ShoppingListItem> _items = new();

    public IEnumerable<ShoppingListItem> Items => _items.ToList();

    public static ShoppingList CreateNew(string name)
    {
        return new ShoppingList
        {
            Id = Guid.NewGuid(),
            Name = name
                .ThrowIfNull()
                .IfEmpty()
                .IfShorterThan(Conventions.NAME_MIN_LENGTH)
                .IfLongerThan(Conventions.NAME_MAX_LENGTH)
        };
    }

    public void AddItem(Guid productId, int amount)
    {
        switch (_items.SingleOrDefault(x => x.ProductId == productId))
        {
            case { } item:
                item.IncreaseAmountBy(amount);
                break;

            case null:
                var newItem = ShoppingListItem.CreateNew(Id, productId, amount);
                _items.Add(newItem);
                EnqueueEvent(new ShoppingListItemAddedEvent
                {
                    ShoppingListId = Id,
                    ProductId = productId,
                    Amount = amount
                });
                break;
        }
    }

    public bool RemoveItem(Guid productId)
    {
        ShoppingListItem? shoppingListItem = _items.SingleOrDefault(x => x.ProductId == productId);
        return shoppingListItem is not null && _items.Remove(shoppingListItem);
    }

    public bool UpdateItem(Guid productId, int amount)
    {
        ShoppingListItem? shoppingListItem = _items.SingleOrDefault(x => x.ProductId == productId);
        if (shoppingListItem is null)
        {
            return false;
        }

        shoppingListItem.SetAmount(amount);
        return true;
    }

    public sealed class Conventions
    {
        public const int NAME_MAX_LENGTH = 100;
        public const int NAME_MIN_LENGTH = 5;
    }
}

public record ShoppingListItemAddedEvent : DomainEvent
{
    public required Guid ShoppingListId { get; init; }
    public required Guid ProductId { get; init; }
    public required int Amount { get; init; }
}
