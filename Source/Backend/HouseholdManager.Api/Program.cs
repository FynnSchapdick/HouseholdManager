using HouseholdManager.Api.Extensions;

await WebApplication.CreateBuilder(args)
    .ConfigureLogging()
    .ConfigureDatabase()
    .ConfigureMessaging()
    .ConfigureSwagger()
    .ConfigureEndpointValidators()
    .ConfigureApi()
    .Build()
    .UseLogging()
    .UseApi()
    .UseVersionedSwagger()
    .UseDevelopmentConfiguration()
    .RunAsync();