using DripChip.Application.Abstractions.Persistence;
using DripChip.Domain.Entities;
using DripChip.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Persistence;

public class ApplicationDbContext :
    IdentityDbContext<User, IdentityRole<int>, int>,
    IApplicationDbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<LocationPoint> LocationPoints => Set<LocationPoint>();
    public DbSet<AnimalType> AnimalTypes => Set<AnimalType>();
    public DbSet<Animal> Animals => Set<Animal>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}