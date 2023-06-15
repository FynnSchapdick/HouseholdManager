namespace HouseholdManager.Api.Domain.Shopping.Events;

public record ShoppingListItemAddedEvent : DomainEvent
{
    public required Guid ShoppingListId { get; init; }
    public required Guid ProductId { get; init; }
    public required int Amount { get; init; }
}