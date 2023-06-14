using HouseholdManager.Api.Data;
using HouseholdManager.Api.Domain;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Consumers;

public sealed class AddProductInfoToShoppingListItemConsumer : IConsumer<ShoppingListItemAddedEvent>
{
    private readonly ShoppingDbContext _shoppingDbContext;
    private readonly ProductDbContext _productDbContext;

    public AddProductInfoToShoppingListItemConsumer(ShoppingDbContext shoppingDbContext, ProductDbContext productDbContext)
    {
        _shoppingDbContext = shoppingDbContext;
        _productDbContext = productDbContext;
    }

    public async Task Consume(ConsumeContext<ShoppingListItemAddedEvent> context)
    {
        ShoppingListItemAddedEvent msg = context.Message;

        Product? product = await _productDbContext.Products.FirstOrDefaultAsync(x => x.Id == msg.ProductId, context.CancellationToken);
        ShoppingList? shoppingList = await _shoppingDbContext.ShoppingLists.AsTracking().FirstOrDefaultAsync(x => x.Id == msg.ShoppingListId, context.CancellationToken);

        if (product is null || shoppingList is null) throw new NotImplementedException();

        ShoppingListItem? item = shoppingList.Items.FirstOrDefault(x => x.ProductId == msg.ProductId);

        if (item is null) throw new NotImplementedException();

        item.SetProductInfo(new ProductInfo(product.Name));

        await _shoppingDbContext.SaveChangesAsync(context.CancellationToken);
    }
}
