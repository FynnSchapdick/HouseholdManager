using FluentValidation;
using Serilog;
using Serilog.Exceptions;
using ShoppingService.Data;
using ShoppingService.Data.Options;
using ShoppingService.Endpoints.CreateShoppingList;

namespace ShoppingService.Extensions;

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
        builder.Services.ConfigureOptions<ShoppingDbOptionsConfiguration>();
        builder.Services.AddDbContext<ShoppingContext>();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateShoppingListRequestValidator>();
        builder.Services.AddSwaggerGen();
        builder.Services.AddEndpointsApiExplorer();
        return builder;
    }
}