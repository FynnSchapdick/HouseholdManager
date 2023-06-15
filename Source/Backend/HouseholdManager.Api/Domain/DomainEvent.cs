namespace HouseholdManager.Api.Domain;

public abstract record DomainEvent
{
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;
}
