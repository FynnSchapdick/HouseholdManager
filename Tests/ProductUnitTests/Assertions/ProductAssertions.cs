using FluentAssertions;
using FluentAssertions.Primitives;
using HouseholdManager.Api.Domain;

namespace ProductUnitTests.Assertions;

public sealed class ProductAssertions : ReferenceTypeAssertions<Product, ProductAssertions>
{
    public ProductAssertions(Product product) : base(product){}
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
    public static ProductAssertions Should(this Product instance) => new(instance);
}