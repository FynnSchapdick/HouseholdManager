using HouseholdManager.Data.Product;
using HouseholdManager.Domain.Product;
using HouseholdManager.Domain.Shopping;
using HouseholdManager.Domain.Shopping.Events;
using HouseholdManager.Domain.Shopping.ValueObjects;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Consumers;

public class AddProductInfoToShoppingListItemConsumerDefinition : ConsumerDefinition<AddProductInfoToShoppingListItemConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<AddProductInfoToShoppingListItemConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseRetry(r => r.Incremental(5, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3)));
    }
}

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

        ProductAggregate? product = await _productDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == msg.ProductId, context.CancellationToken);
        ShoppingListAggregate? shoppingList = await _shoppingListRepository.GetByIdAsync(msg.ShoppingListId, context.CancellationToken);

        if (product is null || shoppingList is null) throw new NotImplementedException();

        ShoppingListItem? item = shoppingList.Items.FirstOrDefault(x => x.ProductId == msg.ProductId);

        if (item is null) throw new NotImplementedException();

        item.SetProductInfo(ProductInfo.Create(product.Name));

        await _shoppingListRepository.SaveAsync(shoppingList, context.CancellationToken);
    }
}