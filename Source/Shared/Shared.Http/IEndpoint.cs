using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Routing;

namespace Shared.Http;

public interface IEndpoint
{
    public static abstract void Configure(IEndpointRouteBuilder builder, [StringSyntax("Route"), RouteTemplate] string route);
}
