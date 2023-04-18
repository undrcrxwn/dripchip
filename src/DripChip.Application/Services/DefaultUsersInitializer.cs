using DripChip.Application.Exceptions;
using DripChip.Application.Features.Accounts.Commands;
using DripChip.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        var users = scope.ServiceProvider.GetRequiredService<UserManager<Account>>();
        
        var commands = _configuration.GetSection("DefaultUsers").Get<IEnumerable<Create.Command>>()!;
        foreach (var command in commands)
        {
            try
            {
                if (await users.FindByIdAsync(command.Id.ToString()) is not null)
                    continue;

                var account = command.Adapt<Account>();
                account.Id = Random.Shared.Next();
                account.UserName = command.Email;
                var result = await users.CreateAsync(account, command.Password);
            }
            catch (AlreadyExistsException) { }
            catch (ValidationException exception)
            {
                //logger.LogError(exception, "Validation failed for default user with ID {0}, Email {1}: {2}",
                //    command.Id, command.Email, string.Join(" ", exception.Errors.SelectMany(error => error.Value)));

                throw;
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}