using ShoppingService.Extensions;

await WebApplication.CreateBuilder(args)
    .AddApi()
    .Build()
    .UseApi()
    .RunAsync();