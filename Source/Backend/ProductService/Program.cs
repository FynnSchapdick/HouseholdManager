using ProductService.Extensions;

await WebApplication.CreateBuilder(args)
    .AddApi()
    .Build()
    .UseApi()
    .RunAsync();
