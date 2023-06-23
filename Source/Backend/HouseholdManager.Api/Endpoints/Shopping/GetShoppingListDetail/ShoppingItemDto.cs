using HouseholdManager.Domain.Shopping.ValueObjects;
using JetBrains.Annotations;

namespace HouseholdManager.Api.Endpoints.Shopping.GetShoppingListDetail;

[UsedImplicitly]
public sealed record ShoppingItemDto(Guid ProductId, int Amount, ProductInfo? ProductInfo)
{
    public static ShoppingItemDto FromDomain(ShoppingListItem item)
    {
        return new ShoppingItemDto(item.ProductId, item.Amount, item.ProductInfo);
    }
}
