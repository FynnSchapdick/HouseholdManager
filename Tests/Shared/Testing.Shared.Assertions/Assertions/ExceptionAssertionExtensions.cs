using FluentAssertions;
using FluentAssertions.Specialized;

namespace Testing.Shared.Assertions.Assertions;

public static class ExceptionAssertionExtensions
{
    public static ExceptionAssertions<T> WhichShouldHaveAMessage<T>(this ExceptionAssertions<T> assertions, string because = "because exceptions without message suck", params object[] becauseArgs) where T : Exception
    {
        assertions.And.Message.Should().NotBeNullOrWhiteSpace(because, becauseArgs);
        return assertions;
    }
}
