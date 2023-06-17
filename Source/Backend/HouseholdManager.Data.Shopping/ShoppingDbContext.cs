using HouseholdManager.Domain.Shopping;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;

namespace HouseholdManager.Data.Shopping;

public sealed class ShoppingDbContext : DbContext, IShoppingListRepository
{
    private readonly IPublishEndpoint _dispatcher;

    public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options, IPublishEndpoint dispatcher) : base(options)
    {
        _dispatcher = dispatcher;
    }

    public DbSet<ShoppingListAggregate> ShoppingLists { get; private set; } = null!;

    public async Task<ShoppingListAggregate?> GetByIdAsync(Guid shoppingListId, CancellationToken cancellationToken = default)
    {
        return await ShoppingLists.FindAsync(new object[] { shoppingListId }, cancellationToken);
    }

    public async Task SaveAsync(ShoppingListAggregate aggregate, CancellationToken cancellationToken = default)
    {
        if (!ChangeTracker.HasChanges())
        {
            throw new NotImplementedException("How did we get here?");
        }

        await SaveChangesAsync(cancellationToken);

        foreach (DomainEvent @event in aggregate.Events)
        {
            await _dispatcher.Publish((object)@event, cancellationToken);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingDbContext).Assembly);
    }
}