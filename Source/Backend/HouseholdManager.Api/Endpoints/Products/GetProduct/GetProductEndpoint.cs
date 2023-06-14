using System.Net;
using System.Net.Mime;
using HouseholdManager.Api.Data;
using HouseholdManager.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Endpoints.Products.GetProduct;

public static class GetProductEndpoint
{
    public const string ENDPOINT_NAME = "GetProductById";

    public static IEndpointRouteBuilder MapGetProductEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("products/{productId:guid}", GetProduct)
            .Produces<ProductDto>(contentType: MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.InternalServerError)
            .Produces((int)HttpStatusCode.NotFound)
            .WithName(ENDPOINT_NAME)
            .WithTags("Products");

        return builder;
    }

    private static async Task<IResult> GetProduct(
        [AsParameters] GetProductParameters parameters,
        ProductDbContext productDbContext,
        CancellationToken cancellationToken)
    {
        try
        {
            Product? product = await productDbContext
                .Products
                .FirstOrDefaultAsync(x => x.Id == parameters.ProductId, cancellationToken);

            return product is null
                ? Results.NotFound(new { ProductId = parameters.ProductId })
                : Results.Ok(new ProductDto(product.Id, product.Name, product.Ean));
        }
        catch (Exception exception)
        {
            return Results.Problem(new ProblemDetails
            {
                Detail = exception.Message,
                Status = (int)HttpStatusCode.InternalServerError
            });
        }
    }
}