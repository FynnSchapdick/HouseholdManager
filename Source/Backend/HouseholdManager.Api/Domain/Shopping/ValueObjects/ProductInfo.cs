using Throw;

namespace HouseholdManager.Api.Domain.Shopping.ValueObjects;

public sealed record ProductInfo
{
    public string ProductName { get; private init; }

    private ProductInfo()
    {

    }

    public static ProductInfo Create(string productName)
    {
        return new ProductInfo()
        {
            ProductName = productName.ThrowIfNull().IfNullOrWhiteSpace(x => x)
        };
    }
}
