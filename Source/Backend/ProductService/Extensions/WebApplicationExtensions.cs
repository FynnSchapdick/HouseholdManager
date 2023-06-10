using ProductService.Endpoints.CreateProduct;
using ProductService.Endpoints.GetProduct;
using Serilog;

namespace ProductService.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        app.NewVersionedApi()
            .MapGroup("/v{version:apiVersion}")
            .HasApiVersion(1.0)
            .MapCreateProductEndpoint()
            .MapGetProductEndpoint();

        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
