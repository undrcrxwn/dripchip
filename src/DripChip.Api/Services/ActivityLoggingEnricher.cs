using System.Diagnostics;
using DripChip.Api.Extensions;
using Serilog.Core;
using Serilog.Events;

namespace DripChip.Api.Services;

public class ActivityLoggingEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;

        logEvent.AddPropertyIfAbsent(new LogEventProperty("SpanIdOptional", AsValue("SPAN", activity?.GetSpanId())));
        logEvent.AddPropertyIfAbsent(new LogEventProperty("TraceIdOptional", AsValue("TRACE", activity?.GetTraceId())));
        logEvent.AddPropertyIfAbsent(new LogEventProperty("ParentIdOptional", AsValue("PARENT", activity?.GetParentId())));

        // If value exists, then return ' PREFIX:VALUE '
        // Otherwise, return ' '
        LogEventPropertyValue AsValue(string prefix, string? value) => new ScalarValue(
            value is not null
                ? $" {prefix}:{activity?.GetSpanId()} "
                : " ");
    }
}