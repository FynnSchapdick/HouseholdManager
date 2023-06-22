using JetBrains.Annotations;

namespace HouseholdManager.Api.Endpoints.Products.GetProduct;

[UsedImplicitly]
public sealed record ProductDto(Guid ProductId, string Name, string? Ean);
