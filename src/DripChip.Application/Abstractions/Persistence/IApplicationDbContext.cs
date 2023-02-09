using Microsoft.EntityFrameworkCore;
using DripChip.Domain.Entities;

namespace DripChip.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    public DbSet<LocationPoint> LocationPoints { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}