using Asp.Versioning;
using FluentValidation;
using HouseholdManager.Api.Data;
using HouseholdManager.Api.Endpoints.Products.CreateProduct;
using HouseholdManager.Api.Endpoints.Shopping.CreateShoppingList;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Exceptions;
using Shared.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HouseholdManager.Api.Extensions;

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

        builder.Services.AddDbContext<ShoppingDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("ShoppingDb"));
            opt.EnableDetailedErrors();
            opt.EnableSensitiveDataLogging();
            opt.UseSnakeCaseNamingConvention();
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        
        builder.Services.AddDbContext<ProductDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("ProductDb"));
            opt.EnableDetailedErrors();
            opt.EnableSensitiveDataLogging();
            opt.UseSnakeCaseNamingConvention();
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        builder.Services.AddValidatorsFromAssemblyContaining<CreateShoppingListRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

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

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

        return builder;
    }
}
