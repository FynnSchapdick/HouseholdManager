using HouseholdManager.Domain.Shopping.Events;
using HouseholdManager.Domain.Shopping.ValueObjects;
using Shared.Domain;
using Throw;

namespace HouseholdManager.Domain.Shopping;

public record ShoppingListAggregate : Aggregate
{
    public Guid ShoppingListId { get; }
    public string Name { get; private set; }

    private readonly HashSet<ShoppingListItem> _items;

    public IEnumerable<ShoppingListItem> Items => _items.ToList();

#pragma warning disable CS8618
    private ShoppingListAggregate()
    {
        /*Ef*/
    }
#pragma warning restore CS8618

    internal ShoppingListAggregate(Guid shoppingListId, string name, HashSet<ShoppingListItem>? items = default)
    {
        ShoppingListId = shoppingListId;
        Name = name
            .ThrowIfNull()
            .IfEmpty()
            .IfWhiteSpace()
            .IfShorterThan(Conventions.NAME_MIN_LENGTH)
            .IfLongerThan(Conventions.NAME_MAX_LENGTH);
        _items = items ?? new HashSet<ShoppingListItem>();
    }

    public static ShoppingListAggregate CreateNew(string name)
    {
        return new ShoppingListAggregate(Guid.NewGuid(), name);
    }

    public bool AddItem(Guid productId, int amount)
    {
        switch (_items.SingleOrDefault(x => x.ProductId == productId))
        {
            case { } item:
                item.IncreaseAmountBy(amount);
                return false;

            case null:
                var newItem = ShoppingListItem.CreateNew(ShoppingListId, productId, amount);
                _items.Add(newItem);
                EnqueueEvent(new ShoppingListItemAddedEvent
                {
                    ShoppingListId = ShoppingListId,
                    ProductId = productId,
                    Amount = amount
                });
                return true;
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

    public override string ToString()
    {
        return $"ShoppingList {ShoppingListId} '{Name}' with {_items.Count} Items";
    }

    public static class Conventions
    {
        public const int NAME_MAX_LENGTH = 100;
        public const int NAME_MIN_LENGTH = 5;
    }
}
