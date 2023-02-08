using DripChip.Api.Filters;
using DripChip.Api.Policies;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DripChip.Api.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddHealthChecks();

        services.AddControllers(options =>
        {
            var transformer = new KebabCaseParameterPolicy();
            options.Conventions.Add(new RouteTokenTransformerConvention(transformer));
            options.Filters.Add<ApiExceptionFilterAttribute>();
        });

        return services;
    }
}