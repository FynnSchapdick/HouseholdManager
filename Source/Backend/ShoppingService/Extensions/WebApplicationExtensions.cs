using Serilog;
using ShoppingService.Endpoints.AddShoppingItem;
using ShoppingService.Endpoints.CreateShoppingList;
using ShoppingService.Endpoints.GetShoppingList;

namespace ShoppingService.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.MapPostShoppingListEndpoint();
        app.MapGetGetShoppingListEndpoint();
        app.MapPostShoppingItemEndpoint();
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
