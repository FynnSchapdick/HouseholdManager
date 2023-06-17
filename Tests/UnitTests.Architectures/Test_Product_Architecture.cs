using HouseholdManager.Domain.Product;
using HouseholdManager.Domain.Shopping;

namespace UnitTests.Architectures;

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