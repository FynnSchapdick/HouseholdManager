using HouseholdManager.Api.Data.Configurations;
using HouseholdManager.Api.Domain;
using HouseholdManager.Api.Domain.Shopping;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Data;

public sealed class ShoppingDbContext : DbContext
{
    private readonly IPublishEndpoint _dispatcher;

    public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options, IPublishEndpoint dispatcher) : base(options)
    {
        _dispatcher = dispatcher;
    }

    public DbSet<ShoppingListAggregate> ShoppingLists { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ShoppingListConfiguration());
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        IEnumerable<Aggregate> aggregates = ChangeTracker.Entries()
            .Where(x => x.Entity.GetType().IsAssignableTo(typeof(Aggregate)))
            .Select(x => (Aggregate)x.Entity);

        int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        foreach (Aggregate aggregate in aggregates)
        {
            foreach (DomainEvent @event in aggregate.Events)
            {
                await _dispatcher.Publish((object)@event, cancellationToken);
            }

            aggregate.ClearEvents();
        }

        return result;
    }
}
