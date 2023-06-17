using Throw;

namespace HouseholdManager.Domain.Shopping.ValueObjects;

public sealed record ProductInfo
{
    public string ProductName { get; private init; }

    internal ProductInfo(string productName)
    {
        ProductName = productName.ThrowIfNull().IfNullOrWhiteSpace(x => x);
    }

#pragma warning disable CS8618
    private ProductInfo()
    {
        /*Ef*/
    }
#pragma warning restore CS8618

    public static ProductInfo Create(string productName)
    {
        return new ProductInfo(productName);
    }
}