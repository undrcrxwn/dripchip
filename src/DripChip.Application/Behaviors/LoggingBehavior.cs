using Mediator;
using Microsoft.Extensions.Logging;

namespace DripChip.Application.Behaviors;

/// <summary>
/// Mediator pipeline behavior that logs every request handling.
/// </summary>
/// <remarks>https://github.com/jbogard/MediatR/wiki/Behaviors</remarks>/>
internal sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var requestType = typeof(TRequest);
        var requestTypeName = requestType.FullName is not null
            // For example, 'Accounts.Commands.Create+Command'
            ? string.Join('.', requestType.FullName.Split('.').TakeLast(3))
            : typeof(TRequest).Name;
        
        _logger.LogInformation("Handling {RequestName} {@Message}", requestTypeName, message);

        var response = await next(message, cancellationToken);

        _logger.LogInformation("Handled {RequestName}", requestTypeName);
        return response;
    }
}