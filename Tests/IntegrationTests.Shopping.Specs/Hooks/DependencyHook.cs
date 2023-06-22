using BoDi;
using Testing.Shared.Setup;
using Xunit.Abstractions;

namespace IntegrationTests.Shopping.Specs.Hooks;

[Binding]
public sealed class DependencyHook
{
    private readonly IObjectContainer _objectContainer;
    private readonly ITestOutputHelper _outputHelper;

    public DependencyHook(IObjectContainer objectContainer, ITestOutputHelper outputHelper)
    {
        _objectContainer = objectContainer;
        _outputHelper = outputHelper;
    }

    [BeforeScenario]
    public async Task SetupDependencies()
    {
        var factory = new HouseholdManagerWebApplicationFactory(_outputHelper);
        _objectContainer.RegisterInstanceAs(factory);
    }
}
