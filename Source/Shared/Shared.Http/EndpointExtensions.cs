using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Routing;

namespace Shared.Http;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder Map<TEndpoint>(this IEndpointRouteBuilder builder, [StringSyntax("Route"), RouteTemplate] string route = "/") where TEndpoint : IEndpoint
    {
        TEndpoint.Configure(builder, route);
        return builder;
    }
}
