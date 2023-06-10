using Microsoft.Extensions.Options;

namespace ProductService.Data.Options;

public sealed class ProductDbOptionsConfiguration : IConfigureOptions<ProductDbOptions>
{
    private const string ProductDb = nameof(ProductDb);

    private readonly IConfiguration _configuration;

    public ProductDbOptionsConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(ProductDbOptions options)
    {
        options.ConnectionString = _configuration.GetConnectionString(ProductDb)
                                   ?? throw new Exception("The Connectionstring for the Product Service is missing");
    }
}
