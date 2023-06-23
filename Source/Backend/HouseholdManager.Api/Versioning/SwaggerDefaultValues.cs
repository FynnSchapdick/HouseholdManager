using System.Text.Json;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HouseholdManager.Api.Versioning;

/// <summary>
/// Represents the OpenAPI/Swashbuckle operation filter used to document information provided, but not used.
/// </summary>
/// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
/// Once they are fixed and published, this class can be removed.</remarks>
[UsedImplicitly]
public sealed class SwaggerDefaultValues : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ApiDescription? apiDescription = context.ApiDescription;
        operation.Deprecated |= apiDescription.IsDeprecated();

        RemoveUnsupportedContentTypes(operation, context.ApiDescription);

        if (operation.Parameters != null)
        {
            UpdateOperationParameters(operation, context.ApiDescription);
        }
    }
    
    private void RemoveUnsupportedContentTypes(OpenApiOperation operation, ApiDescription apiDescription)
    {
        foreach (ApiResponseType responseType in apiDescription.SupportedResponseTypes)
        {
            string responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
            if (!operation.Responses.TryGetValue(responseKey, out OpenApiResponse? response))
            {
                continue;
            }

            List<string> unsupportedContentTypes = response.Content.Keys
                .Where(contentType => responseType.ApiResponseFormats.All(x => x.MediaType != contentType))
                .ToList();

            foreach (string unsupportedContentType in unsupportedContentTypes)
            {
                response.Content.Remove(unsupportedContentType);
            }
        }
    }
    
    private void UpdateOperationParameters(OpenApiOperation operation, ApiDescription apiDescription)
    {
        foreach (OpenApiParameter parameter in operation.Parameters)
        {
            ApiParameterDescription description = apiDescription.ParameterDescriptions
                .First(p => p.Name == parameter.Name);

            parameter.Description ??= description.ModelMetadata.Description;

            if (parameter.Schema.Default is null &&
                description.DefaultValue is not null &&
                description.DefaultValue is not DBNull &&
                description.ModelMetadata is { } modelMetadata)
            {
                string json = JsonSerializer.Serialize(description.DefaultValue, modelMetadata.ModelType);
                parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
            }

            parameter.Required |= description.IsRequired;
        }
    }
}