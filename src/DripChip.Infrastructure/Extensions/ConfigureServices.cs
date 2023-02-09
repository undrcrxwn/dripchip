using System.Reflection;
using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Infrastructure.Identity;
using DripChip.Infrastructure.Identity.Services;
using DripChip.Infrastructure.Persistence;
using DripChip.Infrastructure.Persistence.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DripChip.Infrastructure.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services
            .AddMediatR(assembly)
            .AddHostedService<DatabaseInitializer>()
            .AddSingleton<IFilterFactory, FilterFactory>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddTransient(typeof(Application.Abstractions.Identity.IPasswordValidator<>), typeof(Identity.Services.PasswordValidator<>));
        
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services
            .AddIdentity<Account, IdentityRole<int>>()
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