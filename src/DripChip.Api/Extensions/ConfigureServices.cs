using DripChip.Api.Policies;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DripChip.Api.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            var transformer = new KebabCaseParameterPolicy();
            options.Conventions.Add(new RouteTokenTransformerConvention(transformer));
        });

        return services;
    }
}