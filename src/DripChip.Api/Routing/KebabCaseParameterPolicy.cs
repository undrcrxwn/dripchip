using System.Text.RegularExpressions;

namespace DripChip.Api.Routing;

public class KebabCaseParameterPolicy : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value) => value switch
    {
        not null => Regex.Replace(
            value.ToString() ?? string.Empty,
            "([a-z])([A-Z])", "$1-$2").ToLower(),
        _ => null
    };
}