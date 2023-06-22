using System.Net;
using System.Net.Mime;
using HouseholdManager.Data.Product;
using HouseholdManager.Domain.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Http;

namespace HouseholdManager.Api.Endpoints.Products.GetProduct;

public sealed class GetProductEndpoint : IEndpoint
{
    public const string ENDPOINT_NAME = "GetProductById";

    public static void Configure(IEndpointRouteBuilder builder, string route)
    {
        builder.MapGet(route, GetProduct)
            .Produces<ProductDto>(contentType: MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.InternalServerError)
            .Produces((int)HttpStatusCode.NotFound)
            .WithName(ENDPOINT_NAME)
            .WithTags("Products");
    }

    private static async Task<IResult> GetProduct(
        [AsParameters] GetProductParameters parameters,
        ProductDbContext productDbContext,
        CancellationToken cancellationToken)
    {
        try
        {
            ProductAggregate? product = await productDbContext
                .Products
                .FirstOrDefaultAsync(x => x.ProductId == parameters.ProductId, cancellationToken);

            return product is null
                ? Results.NotFound(new { ProductId = parameters.ProductId })
                : Results.Ok(new ProductDto(product.ProductId, product.Name, product.Ean));
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
