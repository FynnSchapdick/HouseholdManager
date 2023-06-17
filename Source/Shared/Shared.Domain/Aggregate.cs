namespace Shared.Domain;

public abstract record Aggregate
{
    private readonly List<DomainEvent> _events = new();
    public IEnumerable<DomainEvent> Events => _events.OrderBy(x => x.Timestamp);

    protected void EnqueueEvent(DomainEvent @event) => _events.Add(@event);
    public void ClearEvents() => _events.Clear();
}