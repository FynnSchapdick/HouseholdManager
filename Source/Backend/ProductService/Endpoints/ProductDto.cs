namespace ProductService.Endpoints;

public sealed record ProductDto(Guid ProductId, string Name, string? Ean);