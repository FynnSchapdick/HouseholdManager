using HouseholdManager.Data.Product;
using HouseholdManager.Data.Shopping;
using HouseholdManager.Domain.Product;
using HouseholdManager.Domain.Shopping;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Testing.Shared.Setup;

namespace IntegrationTests.Shopping.Specs.Data;

public static class Setup
{
    public static async Task<ShoppingListAggregate> SingleShoppingList(HouseholdManagerWebApplicationFactory factory, string? name = null)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        var dbcontext = scope.ServiceProvider.GetRequiredService<ShoppingDbContext>();

        EntityEntry<ShoppingListAggregate> entry = dbcontext.ShoppingLists.Add(ShoppingListAggregate.CreateNew(name ?? "TestList"));
        await dbcontext.SaveChangesAsync();

        return entry.Entity;
    }

    public static async Task<ProductAggregate> SingleProduct(HouseholdManagerWebApplicationFactory factory, string? name = null)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        var dbcontext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        EntityEntry<ProductAggregate> entry = dbcontext.Products.Add(ProductAggregate.CreateNew(name ?? "TestProduct"));
        await dbcontext.SaveChangesAsync();

        return entry.Entity;
    }
}
