using DripChip.Application.Abstractions.Persistence;
using DripChip.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Persistence;

public sealed class ApplicationDbContext :
    IdentityDbContext<Account, IdentityRole<int>, int>,
    IApplicationDbContext
{
    public DbSet<Account> Accounts => Users;
    public DbSet<LocationPoint> LocationPoints => Set<LocationPoint>();
    public DbSet<AnimalType> AnimalTypes => Set<AnimalType>();
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Visit> AnimalLocationVisits => Set<Visit>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<AreaPoint> AreaPoints => Set<AreaPoint>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Account>().Property(x => x.Id).UseIdentityColumn();
        builder.Entity<AreaPoint>().HasKey(x => new { x.AreaId, x.SequenceId });

        // Relationships
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

        builder.Entity<Area>()
            .HasMany(x => x.AreaPoints)
            .WithOne(x => x.Area);

        builder.Entity<Area>()
            .Navigation(x => x.AreaPoints)
            .AutoInclude();

        base.OnModelCreating(builder);
    }
}