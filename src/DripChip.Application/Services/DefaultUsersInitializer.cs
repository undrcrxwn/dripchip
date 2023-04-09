using DripChip.Application.Abstractions;
using DripChip.Application.Exceptions;
using DripChip.Application.Features.Accounts.Commands;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DripChip.Application.Services;

public class DefaultUsersInitializer : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public DefaultUsersInitializer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DefaultUsersInitializer>>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var issuer = scope.ServiceProvider.GetRequiredService<ICurrentUserProvider>();

        issuer.BypassAuthentication = true;
        
        var commands = _configuration.GetSection("DefaultUsers").Get<IEnumerable<Create.Command>>()!;
        foreach (var command in commands)
        {
            try
            {
                await mediator.Send(command, cancellationToken);
            }
            catch (AlreadyExistsException) { }
            catch (ValidationException exception)
            {
                logger.LogError(exception, "Validation failed for default user with ID {0}, Email {1}: {2}",
                    command.Id, command.Email, string.Join(" ", exception.Errors.SelectMany(error => error.Value)));

                throw;
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}