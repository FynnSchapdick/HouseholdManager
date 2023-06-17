using Asp.Versioning;
using FluentValidation;
using HouseholdManager.Api.Consumers;
using HouseholdManager.Api.Endpoints.Products.CreateProduct;
using HouseholdManager.Api.Endpoints.Shopping.CreateShoppingList;
using HouseholdManager.Data.Product.Extensions;
using HouseholdManager.Data.Shopping.Extensions;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Exceptions;
using Swashbuckle.AspNetCore.SwaggerGen;
using ConfigureSwaggerOptions = HouseholdManager.Api.Versioning.ConfigureSwaggerOptions;
using SwaggerDefaultValues = HouseholdManager.Api.Versioning.SwaggerDefaultValues;

namespace HouseholdManager.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddApiVersioning(v =>
        {
            v.DefaultApiVersion = new ApiVersion(1.0);
            v.AssumeDefaultVersionWhenUnspecified = true;
            v.ReportApiVersions = true;
            v.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
        return builder;
    }

    public static WebApplicationBuilder ConfigureEndpointValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<CreateShoppingListRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
        return builder;
    }

    public static WebApplicationBuilder ConfigureMessaging(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(mt =>
        {
            mt.AddConsumer<AddProductInfoToShoppingListItemConsumer, AddProductInfoToShoppingListItemConsumerDefinition>();
            mt.UsingInMemory((context, configurator) => { context.ConfigureEndpoints(configurator, new SnakeCaseEndpointNameFormatter(false)); });
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.ConfigureProductDbContext("ProductDb", opt =>
        {
            opt.EnableDetailedErrors();
            opt.EnableSensitiveDataLogging();
        });

        builder.ConfigureShoppingDbContext("ShoppingDb", opt =>
        {
            opt.EnableDetailedErrors();
            opt.EnableSensitiveDataLogging();
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
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

        return builder;
    }
}
