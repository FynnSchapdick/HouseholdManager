namespace HouseholdManager.Api.Domain;

public abstract record Aggregate
{
    public Queue<DomainEvent> Events { get; private set; } = new();
    protected void EnqueueEvent(DomainEvent @event) => Events.Enqueue(@event);
}
