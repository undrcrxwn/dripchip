using Microsoft.OpenApi.Models;

namespace DripChip.Api.Extensions;

public static class SwaggerExtensions
{
    private const string SwaggerIgnoredNamespaceIdentifiersKey = "Swagger:IgnoredNamespaceIdentifiers";

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
    {
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
                    .Get<HashSet<string>>()!;

                // Generates unique and user-friendly names for CQRS entities.
                // For example, 'Features.Accounts.Commands.Create.Command' gets turned into 'AccountsCreateCommand'.
                var lastNames = type.FullName!.Split('.', '+')
                    .Where(identifier => !ignoredIdentifiers.Contains(identifier))
                    .TakeLast(3);

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

        return services;
    }
}