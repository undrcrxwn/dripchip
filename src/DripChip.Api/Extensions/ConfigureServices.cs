using DripChip.Api.Filters;
using DripChip.Api.Routing;
using DripChip.Api.Services;
using DripChip.Application.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;
using Serilog;

namespace DripChip.Api.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
            .ConfigureSwagger(configuration)
            .AddEndpointsApiExplorer()
            .AddHealthChecks();
        
        // Logging (Serilog)
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.With<ActivityLoggingEnricher>()
            .CreateLogger();

        // Controllers, naming conventions and request filtering
        services.AddControllers(options =>
        {
            var transformer = new KebabCaseParameterPolicy();
            options.Conventions.Add(new RouteTokenTransformerConvention(transformer));
            options.Filters.Add<ApiExceptionFilterAttribute>();
            options.Filters.Add<ApiAuthorizationFilter>();
        });
        
        // Versioning
        services.AddApiVersioning(options => {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });
        
        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        // Security (authentication and authorization)
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = "BasicAuthentication";
                options.DefaultAuthenticateScheme = "BasicAuthentication";
                options.DefaultChallengeScheme = "BasicAuthentication";
                options.DefaultForbidScheme = "BasicAuthentication";
            })
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
                authenticationScheme: "BasicAuthentication",
                displayName: "Basic Authentication",
                configureOptions: null);

        services.AddAuthorization(options => options.DefaultPolicy =
            new AuthorizationPolicyBuilder("BasicAuthentication")
                .RequireAuthenticatedUser()
                .Build());

        return services;
    }
}