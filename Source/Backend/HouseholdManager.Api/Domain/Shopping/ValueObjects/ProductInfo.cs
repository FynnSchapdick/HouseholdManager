using Throw;

namespace HouseholdManager.Api.Domain.Shopping.ValueObjects;

public sealed record ProductInfo
{
    public string ProductName { get; }

    public ProductInfo(string productName)
    {
        ProductName = productName.ThrowIfNull().IfNullOrWhiteSpace(x => x);
    }
}
