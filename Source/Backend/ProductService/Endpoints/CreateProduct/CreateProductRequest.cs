namespace ProductService.Endpoints.CreateProduct;

public record CreateProductRequest(string Name, string? Ean = null);