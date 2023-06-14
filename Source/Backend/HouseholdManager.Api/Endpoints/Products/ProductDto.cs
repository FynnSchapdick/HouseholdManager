namespace HouseholdManager.Api.Endpoints.Products;

public sealed record ProductDto(Guid ProductId, string Name, string? Ean);