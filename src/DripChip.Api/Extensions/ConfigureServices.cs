using DripChip.Api.Filters;
using DripChip.Api.Routing;
using DripChip.Api.Services;
using DripChip.Application.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DripChip.Api.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddHealthChecks();

        services.AddControllers(options =>
        {
            var transformer = new KebabCaseParameterPolicy();
            options.Conventions.Add(new RouteTokenTransformerConvention(transformer));
            options.Filters.Add<ApiExceptionFilterAttribute>();
        });

        services
            .AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
                authenticationScheme: "BasicAuthentication",
                displayName: "Basic authentication",
                configureOptions: null);

        services.AddAuthorization(options => options.DefaultPolicy =
            new AuthorizationPolicyBuilder("BasicAuthentication")
                .RequireAuthenticatedUser()
                .Build());

        return services;
    }
}