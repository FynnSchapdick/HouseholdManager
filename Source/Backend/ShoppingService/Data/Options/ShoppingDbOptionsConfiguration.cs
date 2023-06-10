using Microsoft.Extensions.Options;

namespace ShoppingService.Data.Options;

public sealed class ShoppingDbOptionsConfiguration : IConfigureOptions<ShoppingDbOptions>
{
    private const string ShoppingDb = "ShoppingDb";

    private readonly IConfiguration _configuration;

    public ShoppingDbOptionsConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(ShoppingDbOptions options)
    {
        options.ConnectionString = _configuration.GetConnectionString(ShoppingDb)
                                   ?? throw new Exception("Missing Required ShoppingDb Configuration");
    }
}