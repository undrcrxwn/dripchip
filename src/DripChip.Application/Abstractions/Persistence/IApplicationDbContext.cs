using Microsoft.EntityFrameworkCore;
using DripChip.Domain.Entities;

namespace DripChip.Application.Abstractions.Persistence;

/// <summary>
/// Data access EF Core-dependent abstraction for infrastructure-level persistence concerns.
/// </summary>
public interface IApplicationDbContext
{
    public DbSet<Account> Accounts { get; }
    public DbSet<LocationPoint> LocationPoints { get; }
    public DbSet<AnimalType> AnimalTypes { get; }
    public DbSet<Animal> Animals { get; }
    public DbSet<Visit> AnimalLocationVisits { get; }
    public DbSet<Area> Areas { get; }
    public DbSet<AreaPoint> AreaPoints { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}