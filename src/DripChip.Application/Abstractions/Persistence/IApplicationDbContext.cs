using Microsoft.EntityFrameworkCore;
using DripChip.Domain.Entities;

namespace DripChip.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    public DbSet<LocationPoint> LocationPoints { get; }
    public DbSet<AnimalType> AnimalTypes { get; }
    public DbSet<Animal> Animals { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}