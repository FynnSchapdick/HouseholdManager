using ProductService.Endpoints.CreateProduct;
using ProductService.Endpoints.GetProduct;
using Serilog;

namespace ProductService.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.MapCreateProductEndpoint();
        app.MapGetGetProductEndpoint();
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}