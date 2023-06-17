using HouseholdManager.Domain.Product;
using HouseholdManager.Domain.Shopping;

namespace UnitTests.Architectures;

public sealed class Test_Shopping_Architecture
{
    private static readonly Architecture ShoppingArchitecture = new ArchLoader().LoadAssembly(typeof(ShoppingListAggregate).Assembly).Build();

    [Fact]
    public void ShoppingDomain_Should_HaveNoDependenciesOn_ProductDomain()
    {
        Types().That().ResideInNamespace(typeof(ShoppingListAggregate).Namespace + ".*", true)
            .Should().NotDependOnAnyTypesThat().ResideInNamespace(typeof(ProductAggregate).Namespace).Check(ShoppingArchitecture);
    }
}
