using System.Reflection;
using DripChip.Application.Behaviors;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace DripChip.Application.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(assembly);
        
        return services
            .AddValidatorsFromAssembly(assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
    }
}