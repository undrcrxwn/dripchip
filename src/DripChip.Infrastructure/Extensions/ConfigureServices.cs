using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Abstractions.Specifications;
using DripChip.Infrastructure.Identity;
using DripChip.Infrastructure.Identity.Services;
using DripChip.Infrastructure.Persistence;
using DripChip.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DripChip.Infrastructure.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHostedService<DataStoreInitializer>()
            .AddHostedService<DefaultUsersInitializer>()
            .AddSingleton<ISpecificationFactory, SpecificationFactory>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddTransient(typeof(Application.Abstractions.Identity.IPasswordValidator<>), typeof(Identity.Services.PasswordValidator<>));

        var host = configuration["POSTGRES_HOST"];
        var port = configuration["POSTGRES_PORT"];
        var database = configuration["POSTGRES_DB"];
        var username = configuration["POSTGRES_USER"];
        var password = configuration["POSTGRES_PASSWORD"];

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseNpgsql($"Host={host};Port={port};Database={database};Username={username};Password={password}"));

        services
            .AddIdentity<User, IdentityRole<int>>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 5;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
        });
    }
}