using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Domain;
using ProductService.Endpoints.GetProduct;
using Shared.Validation;

namespace ProductService.Endpoints.CreateProduct;

public static class CreateProductEndpoint
{
    private const string ProductsSwaggerTag = "Products";
    
    public static void MapCreateProductEndpoint(this WebApplication app)
    {
        app.MapPost("products", CreateProduct)
            .Accepts<CreateProductRequest>(MediaTypeNames.Application.Json)
            .Produces((int) HttpStatusCode.Created)
            .Produces((int) HttpStatusCode.Conflict)
            .Produces((int) HttpStatusCode.BadRequest)
            .Produces((int) HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateProductRequest>>()
            .WithTags(ProductsSwaggerTag);
    }

    private static async Task<IResult> CreateProduct(CreateProductRequest request, ProductContext productContext, CancellationToken cancellationToken)
    {
        try
        {
            Product product = Product.CreateNew(request.Name, request.Ean);
            await productContext.AddAsync(product, cancellationToken);
            await productContext.SaveChangesAsync(cancellationToken);
            return Results.CreatedAtRoute(GetProductEndpoint.InternalRouteName, new { ProductId = product.Id });
        }
        catch (DbUpdateException dbUpdateException)
        {
            return Results.Conflict(dbUpdateException.InnerException?.Message);
        }
        catch (Exception exception) when (exception is not ArgumentException)
        {
            return Results.Problem(new ProblemDetails { Detail = exception.Message, Status = (int) HttpStatusCode.InternalServerError });
        }
    }
}