using DripChip.Api.Attributes;
using DripChip.Api.Routing;
using DripChip.Api.Services;
using DripChip.Application.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;

namespace DripChip.Api.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
            .AddEndpointsApiExplorer()
            .AddHealthChecks();

        services.AddControllers(options =>
        {
            var transformer = new KebabCaseParameterPolicy();
            options.Conventions.Add(new RouteTokenTransformerConvention(transformer));
            options.Filters.Add<ApiExceptionFilterAttribute>();
        });

        services.AddSwaggerGen(options => options.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
        {
            Description = "Basic Authentication",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "basic",
            Type = SecuritySchemeType.Http
        }));

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