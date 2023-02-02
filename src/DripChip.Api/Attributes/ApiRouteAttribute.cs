using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Attributes;

public class ApiRouteAttribute : RouteAttribute
{
    private const string Prefix = "api";
    
    public ApiRouteAttribute() : base(Prefix) { }
    public ApiRouteAttribute(
        [StringSyntax("Route")] string template)
        : base($"{Prefix}/{template}") { }
}