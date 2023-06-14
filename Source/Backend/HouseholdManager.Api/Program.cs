using HouseholdManager.Api.Extensions;

await WebApplication.CreateBuilder(args)
    .AddApi()
    .Build()
    .UseApi()
    .RunAsync();