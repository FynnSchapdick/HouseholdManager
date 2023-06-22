using System.Net;
using System.Net.Mime;
using HouseholdManager.Api.Endpoints.Products.GetProduct;
using HouseholdManager.Data.Product;
using HouseholdManager.Domain.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Http;

namespace HouseholdManager.Api.Endpoints.Products.CreateProduct;

public sealed class CreateProductEndpoint : IEndpoint
{
    public static void Configure(IEndpointRouteBuilder builder, string route)
    {
        builder.MapPost(route, CreateProduct)
            .Accepts<CreateProductRequest>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.Created)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateProductRequest>>()
            .WithTags("Products");
    }

    private static async Task<IResult> CreateProduct(CreateProductRequest request, ProductDbContext productDbContext, CancellationToken cancellationToken)
    {
        try
        {
            var product = ProductAggregate.CreateNew(request.Name, request.Ean);
            productDbContext.Add(product);
            await productDbContext.SaveChangesAsync(cancellationToken);
            return Results.CreatedAtRoute(GetProductEndpoint.ENDPOINT_NAME, new GetProductParameters(product.ProductId));
        }
        catch (DbUpdateException dbUpdateException)
        {
            return Results.Problem(new ProblemDetails
            {
                Detail = dbUpdateException.InnerException?.Message ?? dbUpdateException.Message,
                Status = (int)HttpStatusCode.Conflict
            });
        }
        catch (Exception exception) when (exception is not ArgumentException)
        {
            return Results.Problem(new ProblemDetails
            {
                Detail = exception.Message,
                Status = (int)HttpStatusCode.InternalServerError
            });
        }
    }
}
