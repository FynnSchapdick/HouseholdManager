using HouseholdManager.Domain.Shopping.Events;
using HouseholdManager.Domain.Shopping.ValueObjects;
using Shared.Domain;
using Throw;

namespace HouseholdManager.Domain.Shopping;

public record ShoppingListAggregate : Aggregate
{
    public Guid Id { get; }
    public string Name { get; private set; }

    private readonly HashSet<ShoppingListItem> _items;

    public IEnumerable<ShoppingListItem> Items => _items.ToList();

    internal ShoppingListAggregate(Guid id, string name, HashSet<ShoppingListItem>? items = default)
    {
        Id = id;
        Name = name
            .ThrowIfNull()
            .IfEmpty()
            .IfWhiteSpace()
            .IfShorterThan(Conventions.NAME_MIN_LENGTH)
            .IfLongerThan(Conventions.NAME_MAX_LENGTH);
        _items = items ?? new HashSet<ShoppingListItem>();
    }

#pragma warning disable CS8618
    private ShoppingListAggregate()
    {
        /*Ef*/
    }
#pragma warning restore CS8618

    public static ShoppingListAggregate CreateNew(string name)
    {
        return new ShoppingListAggregate(Guid.NewGuid(), name);
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

    public static class Conventions
    {
        public const int NAME_MAX_LENGTH = 100;
        public const int NAME_MIN_LENGTH = 5;
    }
}