using DripChip.Application.Abstractions.Persistence;
using DripChip.Domain.Entities;
using DripChip.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Persistence;

public class ApplicationDbContext :
    IdentityDbContext<Account, IdentityRole<int>, int>,
    IApplicationDbContext
{
    public DbSet<LocationPoint> LocationPoints => Set<LocationPoint>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}