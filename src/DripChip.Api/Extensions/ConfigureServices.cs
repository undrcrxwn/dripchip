using DripChip.Api.Filters;
using DripChip.Api.Routing;
using DripChip.Api.Services;
using DripChip.Application.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;
using Serilog;

namespace DripChip.Api.Extensions;

public static class ConfigureServices
{
    private const string SwaggerIgnoredNamespaceIdentifiersKey = "Swagger:IgnoredNamespaceIdentifiers";

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
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

        // Swagger UI
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "DripChip API", Version = "v1" });

            options.CustomSchemaIds(type =>
            {
                // Ignored parts of namespaces, generally CQRS-conventional names,
                // such as 'Queries' and 'Commands'. These are skipped when generating
                // Swagger names for the public DTOs.
                var ignoredIdentifiers = configuration
                    .GetSection(SwaggerIgnoredNamespaceIdentifiersKey)
                    .Get<string[]>()!;

                // Generates unique and user-friendly names for CQRS entities.
                // For example, 'Features.Accounts.Commands.Create.Command' gets turned into 'AccountsCreateCommand'.
                var lastNames = type.FullName!.Split('.')
                    .Except(ignoredIdentifiers)
                    .TakeLast(2)
                    .Select(name => name.Replace("+", string.Empty));

                return string.Join(string.Empty, lastNames);
            });

            // Enables Swagger UI authorization features
            options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        }
                    },
                    Array.Empty<string>()
                }
            });
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