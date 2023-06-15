using HouseholdManager.Api.Data;
using HouseholdManager.Api.Domain;
using HouseholdManager.Api.Domain.Product;
using HouseholdManager.Api.Domain.Shopping;
using HouseholdManager.Api.Domain.Shopping.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Consumers;

public sealed class AddProductInfoToShoppingListItemConsumer : IConsumer<ShoppingListItemAddedEvent>
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly ProductDbContext _productDbContext;

    public AddProductInfoToShoppingListItemConsumer(IShoppingListRepository shoppingListRepository, ProductDbContext productDbContext)
    {
        _shoppingListRepository = shoppingListRepository;
        _productDbContext = productDbContext;
    }

    public async Task Consume(ConsumeContext<ShoppingListItemAddedEvent> context)
    {
        ShoppingListItemAddedEvent msg = context.Message;

        Product? product = await _productDbContext.Products.FirstOrDefaultAsync(x => x.Id == msg.ProductId, context.CancellationToken);
        ShoppingListAggregate? shoppingList = await _shoppingListRepository.GetByIdAsync(msg.ShoppingListId, context.CancellationToken);

        if (product is null || shoppingList is null) throw new NotImplementedException();

        ShoppingListItem? item = shoppingList.Items.FirstOrDefault(x => x.ProductId == msg.ProductId);

        if (item is null) throw new NotImplementedException();

        item.SetProductInfo(new ProductInfo(product.Name));

        await _shoppingListRepository.SaveAsync(shoppingList, context.CancellationToken);
    }
}
