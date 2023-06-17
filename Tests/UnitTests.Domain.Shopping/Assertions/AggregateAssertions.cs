using FluentAssertions;
using FluentAssertions.Primitives;
using Shared.Domain;

namespace UnitTests.Domain.Shopping.Assertions;

public sealed class AggregateAssertions<T> : ReferenceTypeAssertions<T, AggregateAssertions<T>> where T : Aggregate
{
    public AggregateAssertions(T subject) : base(subject)
    {
    }

    protected override string Identifier => typeof(T).Name.ToLower();

    public AndConstraint<T> NotContainEvents(string because = "", params object[] becauseArgs)
    {
        Subject.Events.Should().BeEmpty(because, becauseArgs);
        return new AndConstraint<T>(Subject);
    }

    public AndWhichConstraint<T, TEvent> ContainSingleEvent<TEvent>(string because = "", params object[] becauseArgs) where TEvent : DomainEvent
    {
        DomainEvent domainEvent = Subject.Events.Should().ContainSingle(because, becauseArgs).Which;
        domainEvent.Should().BeAssignableTo<TEvent>();
        return new AndWhichConstraint<T, TEvent>(Subject, (TEvent)domainEvent);
    }
}