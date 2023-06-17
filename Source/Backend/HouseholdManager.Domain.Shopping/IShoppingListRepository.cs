namespace HouseholdManager.Domain.Shopping;

public interface IShoppingListRepository
{
    public Task<ShoppingListAggregate?> GetByIdAsync(Guid shoppingListId, CancellationToken cancellationToken = default);
    public Task SaveAsync(ShoppingListAggregate aggregate, CancellationToken cancellationToken = default);
}