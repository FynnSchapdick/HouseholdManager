using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using HouseholdManager.Api.Domain.Product;
using HouseholdManager.Api.Domain.Shopping;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ShoppingUnitTests;

public sealed class Test_ShoppingList_Architecture
{
    private static readonly Architecture ShoppingArchitecture = new ArchLoader().LoadAssembly(typeof(ShoppingListAggregate).Assembly).Build();

    [Fact]
    public void ShoppingDomain_Should_HaveNoDependenciesOn_ProductDomain()
    {
        Types().That().ResideInNamespace(typeof(ShoppingListAggregate).Namespace + ".*", true)
            .Should().NotDependOnAnyTypesThat().ResideInNamespace(typeof(ProductAggregate).Namespace).Check(ShoppingArchitecture);
    }
}
