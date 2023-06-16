using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using HouseholdManager.Domain.Product;
using HouseholdManager.Domain.Shopping;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ProductUnitTests;

public sealed class Test_Product_Architecture
{
    private static readonly Architecture ProductArchitecture = new ArchLoader().LoadAssembly(typeof(ProductAggregate).Assembly).Build();

    [Fact]
    public void ProductDomain_Should_HaveNoDependenciesOn_ShoppingDomain()
    {
        Types().That().ResideInNamespace(typeof(ProductAggregate).Namespace + ".*", true)
            .Should().NotDependOnAnyTypesThat().ResideInNamespace(typeof(ShoppingListAggregate).Namespace).Check(ProductArchitecture);
    }
}
