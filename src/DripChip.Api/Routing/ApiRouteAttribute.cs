using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Routing;

/// <summary>
/// A route attribute responsible for applying a default template containing prefix to the given controller or endpoint route.
/// The default pattern is /api/{route}
/// If a route that starts with '~/' is provided, adds the default '/api/v{version}' prefix to the beginning.
/// </summary>
internal sealed class ApiRouteAttribute : RouteAttribute
{
    private const string Prefix = "api/v{version:apiVersion}";
    
    public ApiRouteAttribute() : base(Prefix) { }
    
    public ApiRouteAttribute(string template)
        : base(template.StartsWith("~/")
            ? $"~/{Prefix}/{template[2..]}"
            : $"{Prefix}/{template}") { }
}