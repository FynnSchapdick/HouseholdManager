using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HouseholdManager.Api.Versioning;

/// <summary>
/// Configures the Swagger generation options.
/// </summary>
/// <remarks>This allows API versioning to define a Swagger document per API version after the
/// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
    /// </summary>
    /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        OpenApiInfo info = new OpenApiInfo
        {
            Title = "Household Manager API",
            Version = description.ApiVersion.ToString(),
            Description = GetDescription(description)
        };

        return info;
    }

    private static string GetDescription(ApiVersionDescription description)
    {
        StringBuilder text = new StringBuilder();

        if (description.IsDeprecated)
        {
            text.Append(" This API version has been deprecated.");
        }

        if (description.SunsetPolicy is { } policy)
        {
            AppendSunsetPolicyDetails(text, policy);
        }

        return text.ToString();
    }
    
    private static void AppendSunsetPolicyDetails(StringBuilder text, SunsetPolicy sunsetPolicy)
    {
        if (sunsetPolicy.Date is { } when)
        {
            text.Append(" The API will be sunset on ")
                .Append(when.Date.ToShortDateString())
                .Append('.');
        }

        if (!sunsetPolicy.HasLinks)
        {
            return;
        }

        text.AppendLine();

        foreach (LinkHeaderValue link in sunsetPolicy.Links.Where(x => x.Type == "text/html"))
        {
            text.AppendLine();

            if (link.Title.HasValue)
            {
                text.Append(link.Title.Value).Append(": ");
            }

            text.Append(link.LinkTarget.OriginalString);
        }
    }
}