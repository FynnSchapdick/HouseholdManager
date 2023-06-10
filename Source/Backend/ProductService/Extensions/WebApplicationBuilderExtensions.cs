using FluentValidation;
using ProductService.Data;
using ProductService.Data.Options;
using ProductService.Endpoints.CreateProduct;
using Serilog;
using Serilog.Exceptions;

namespace ProductService.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .WriteTo.Console();
        });
        builder.Services.ConfigureOptions<ProductDbOptionsConfiguration>();
        builder.Services.AddDbContext<ProductDbContext>();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
        builder.Services.AddSwaggerGen();
        builder.Services.AddEndpointsApiExplorer();
        return builder;
    }
}