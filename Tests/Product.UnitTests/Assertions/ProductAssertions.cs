using FluentAssertions;
using FluentAssertions.Primitives;
using HouseholdManager.Api.Domain;

namespace Product.UnitTests.Assertions;

public sealed class ProductAssertions : ReferenceTypeAssertions<HouseholdManager.Api.Domain.Product, ProductAssertions>
{
    public ProductAssertions(HouseholdManager.Api.Domain.Product product) : base(product){}
    protected override string Identifier => "product";

    public AndConstraint<ProductAssertions> HaveName(string name, string because = "", params object[] becauseArgs)
    {
        Subject.Name.Should().Be(name, because, becauseArgs);
        return new AndConstraint<ProductAssertions>(this);
    }
    
    public AndConstraint<ProductAssertions> HaveEan(string ean, string because = "", params object[] becauseArgs)
    {
        Subject.Ean.Should().Be(ean, because, becauseArgs);
        return new AndConstraint<ProductAssertions>(this);
    }
    
    public AndConstraint<ProductAssertions> NotHaveDefaultId(string because = "", params object[] becauseArgs)
    {
        Subject.Id.Should().NotBeEmpty(because, becauseArgs);
        return new AndConstraint<ProductAssertions>(this);
    }
}

public static class ProductAssertionsExtensions
{
    public static ProductAssertions Should(this HouseholdManager.Api.Domain.Product instance) => new(instance);
}