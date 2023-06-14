namespace HouseholdManager.Api.Endpoints.Products.CreateProduct;

public record CreateProductRequest(string Name, string? Ean = null);