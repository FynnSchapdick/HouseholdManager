using Throw;

namespace HouseholdManager.Domain.Shopping.ValueObjects;

public sealed record ProductInfo
{
    public string ProductName { get; }

    internal ProductInfo(string productName)
    {
        ProductName = productName.ThrowIfNull().IfNullOrWhiteSpace(x => x);
    }

    public static ProductInfo Create(string productName)
    {
        return new ProductInfo(productName);
    }
}
