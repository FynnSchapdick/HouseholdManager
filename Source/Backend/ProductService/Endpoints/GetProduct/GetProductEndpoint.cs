using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Domain;

namespace ProductService.Endpoints.GetProduct;

public static class GetProductEndpoint
{
    private const string GetProductRoute = "products/{productId:guid}";
    public const string InternalRouteName = "GetProductById";
    private const string ProductsSwaggerTag = "Products";
    
    public static void MapGetGetProductEndpoint(this WebApplication app)
    {
        app.MapGet(GetProductRoute, GetProduct)
            .Produces<ProductDto>(contentType: MediaTypeNames.Application.Json)
            .Produces((int) HttpStatusCode.InternalServerError)
            .Produces((int) HttpStatusCode.NotFound)
            .WithName(InternalRouteName)
            .WithTags(ProductsSwaggerTag);
    }

    private static async Task<IResult> GetProduct(Guid productId, ProductContext productContext,
        CancellationToken cancellationToken)
    {
        try
        {
            Product? product = await productContext
                .Products
                .FirstOrDefaultAsync(x => x.Id == productId, cancellationToken);

            return product is null
                ? Results.NotFound(new { ProductId = productId })
                : Results.Ok(new ProductDto(product.Id, product.Name, product.Ean));
        }
        catch (Exception exception)
        {
            return Results.Problem(new ProblemDetails
                {Detail = exception.Message, Status = (int) HttpStatusCode.InternalServerError});
        }
    }
}