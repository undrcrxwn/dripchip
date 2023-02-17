using DripChip.Application.Abstractions.Persistence;
using DripChip.Domain.Entities;
using DripChip.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Persistence;

public sealed class ApplicationDbContext :
    IdentityDbContext<User, IdentityRole<int>, int>,
    IApplicationDbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<LocationPoint> LocationPoints => Set<LocationPoint>();
    public DbSet<AnimalType> AnimalTypes => Set<AnimalType>();
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Visit> AnimalLocationVisits => Set<Visit>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) => ChangeTracker.LazyLoadingEnabled = false;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Animal>()
            .HasMany(x => x.AnimalTypes)
            .WithMany(x => x.Animals);
        
        builder.Entity<Animal>()
            .HasOne(x => x.Chipper)
            .WithMany(x => x.ChippedAnimals);
        
        builder.Entity<Animal>()
            .HasOne(x => x.ChippingLocation)
            .WithMany(x => x.ChippedAnimals);
        
        builder.Entity<Animal>()
            .HasMany(x => x.VisitedLocations)
            .WithOne(x => x.Visitor);
        
        base.OnModelCreating(builder);
    }
}