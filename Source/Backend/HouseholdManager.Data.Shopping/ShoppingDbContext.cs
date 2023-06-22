using HouseholdManager.Domain.Shopping;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Domain;

namespace HouseholdManager.Data.Shopping;

public sealed class ShoppingDbContext : DbContext, IShoppingListRepository
{
    private readonly IPublishEndpoint _dispatcher;
    private readonly ILogger<ShoppingDbContext> _logger;

    public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options, IPublishEndpoint dispatcher, ILogger<ShoppingDbContext> logger) : base(options)
    {
        _dispatcher = dispatcher;
        _logger = logger;
    }

    public DbSet<ShoppingListAggregate> ShoppingLists { get; private set; } = null!;

    public async Task<ShoppingListAggregate?> GetByIdAsync(Guid shoppingListId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Shopping List for {ShoppingListId}", shoppingListId);

        ShoppingListAggregate? shoppingList = await ShoppingLists.AsTracking().SingleOrDefaultAsync(x => x.ShoppingListId == shoppingListId, cancellationToken);

        _logger.LogInformation("Successfully fetched Shopping List {ShoppingList}", shoppingList);

        return shoppingList;
    }

    public async Task RemoveAsync(ShoppingListAggregate aggregate, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting Shopping List {ShoppingList}", aggregate);

        ShoppingLists.Remove(aggregate);
        await SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sucessfully deleted Shopping List {ShoppingList}", aggregate);
    }

    public Task SaveNewAsync(ShoppingListAggregate aggregate, CancellationToken cancellationToken = default)
    {
        Add(aggregate);
        return SaveAsync(aggregate, cancellationToken);
    }

    public async Task SaveAsync(ShoppingListAggregate aggregate, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving Shopping List {ShoppingList}", aggregate);

        if (!ChangeTracker.HasChanges())
        {
            _logger.LogCritical("Saving the Shopping List {ShoppingList} failed since no changes were detected", aggregate);
            throw new NotImplementedException("How did we get here?");
        }

        await SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sucessfully saved Shopping List {ShoppingList}", aggregate);

        foreach (DomainEvent @event in aggregate.Events)
        {
            _logger.LogDebug("Dispatching {Event}", @event);
            await _dispatcher.Publish((object)@event, cancellationToken);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingDbContext).Assembly);
    }
}
