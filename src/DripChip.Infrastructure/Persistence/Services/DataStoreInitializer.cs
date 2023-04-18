using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DripChip.Infrastructure.Persistence.Services;

/// <summary>
/// Hosted services that initializes database on application start-up if it has not been initialized yet.
/// </summary>
internal class DataStoreInitializer : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DataStoreInitializer(IServiceScopeFactory scopeFactory) =>
        _scopeFactory = scopeFactory;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await context.Database.EnsureDeletedAsync(cancellationToken);
        await context.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}