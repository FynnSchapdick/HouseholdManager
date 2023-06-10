using Asp.Versioning;
using FluentValidation;
using Microsoft.Extensions.Options;
using ProductService.Data;
using ProductService.Data.Options;
using ProductService.Endpoints.CreateProduct;
using Serilog;
using Serilog.Exceptions;
using Shared.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;

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
