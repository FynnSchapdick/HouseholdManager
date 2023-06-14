using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mime;
using HouseholdManager.Api.Data;
using HouseholdManager.Api.Domain;
using HouseholdManager.Api.Endpoints.Products.GetProduct;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseholdManager.Api.Endpoints.Products.CreateProduct;

public static class CreateProductEndpoint
{
    public static IEndpointRouteBuilder MapCreateProductEndpoint(this IEndpointRouteBuilder builder, [StringSyntax("Route"), RouteTemplate] string route)
    {
        builder.MapPost(route, CreateProduct)
            .Accepts<CreateProductRequest>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.Created)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateProductRequest>>()
            .WithTags("Products");

        return builder;
    }

    private static async Task<IResult> CreateProduct(CreateProductRequest request, ProductDbContext productDbContext, CancellationToken cancellationToken)
    {
        try
        {
            Product product = Product.CreateNew(request.Name, request.Ean);
            productDbContext.Add(product);
            await productDbContext.SaveChangesAsync(cancellationToken);
            return Results.CreatedAtRoute(GetProductEndpoint.ENDPOINT_NAME, new GetProductParameters(product.Id));
        }
        catch (DbUpdateException dbUpdateException)
        {
            return Results.Problem(new ProblemDetails
            {
                Detail = dbUpdateException.InnerException?.Message ?? dbUpdateException.Message,
                Status = (int) HttpStatusCode.Conflict
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
