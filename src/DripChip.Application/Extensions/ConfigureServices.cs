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
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
            // Ignore the 'Cannot resolve symbol' error.
            // It is fake and caused by the fact Rider does not yet support some of .NET 7 features. 
            // See https://github.com/martinothamar/Mediator/issues/64
            .AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
    }
}