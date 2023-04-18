using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DripChip.Application.Features.Areas.Queries;

public static class GetAnalytics
{
    public sealed record Query(long Id, DateTimeOffset StartDate, DateTimeOffset EndDate) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id).AreaId();
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        }
    }

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<Handler> _logger;

        public Handler(IApplicationDbContext context, ILogger<Handler> logger)
        {
            _context = context;
            _logger = logger;
        }

        enum AreaMigrationKind
        {
            None,
            Idle,
            Arrived,
            Gone
        }

        public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var area =
                await _context.Areas.FindAsync(request.Id)
                ?? throw new NotFoundException();

            var polygon = area.ToPolygon();

            var animalsAtStart = await _context.Animals
                .Include(animal => animal.AnimalTypes)
                .Include(animal => animal.ChippingLocation)
                .Include(animal => animal.VisitedLocations)
                .ThenInclude(visit => visit.LocationPoint)
                .Where(animal => animal.ChippingDateTime <= request.StartDate)
                .ToListAsync(cancellationToken);

            animalsAtStart = animalsAtStart
                .Select(animal => new
                {
                    Animal = animal,
                    LastLocation =
                        animal.VisitedLocations
                            .LastOrDefault(visit => visit.DateTimeOfVisitLocationPoint <= request.StartDate)?.LocationPoint
                        ?? animal.ChippingLocation
                })
                .Where(x => x.LastLocation.ToPoint().Overlays(polygon))
                .Select(x => x.Animal)
                .ToList();

            var animalsAtEnd = await _context.Animals
                .Include(animal => animal.AnimalTypes)
                .Include(animal => animal.ChippingLocation)
                .Include(animal => animal.VisitedLocations)
                .ThenInclude(visit => visit.LocationPoint)
                .Where(animal => animal.ChippingDateTime <= request.EndDate)
                .ToListAsync(cancellationToken);

            animalsAtEnd = animalsAtEnd
                .Select(animal => new
                {
                    Animal = animal,
                    LastLocation =
                        animal.VisitedLocations
                            .LastOrDefault(visit => visit.DateTimeOfVisitLocationPoint <= request.EndDate)?.LocationPoint
                        ?? animal.ChippingLocation
                })
                .Where(x => x.LastLocation.ToPoint().Overlays(polygon))
                .Select(x => x.Animal)
                .ToList();

            var animalMigrations = new[] { animalsAtStart, animalsAtEnd }.SelectMany(x => x).Select(animal =>
            {
                var trackedAtStart = animalsAtStart.Any(x => x.Id == animal.Id);
                var trackedAtEnd = animalsAtStart.Any(x => x.Id == animal.Id);

                return new
                {
                    Animal = animal,
                    MigrationKind = (trackedAtStart, trackedAtEnd) switch
                    {
                        (true, true) => AreaMigrationKind.Idle,
                        (true, false) => AreaMigrationKind.Gone,
                        (false, true) => AreaMigrationKind.Arrived,
                        _ => AreaMigrationKind.None
                    }
                };
            }).ToList();

            _logger.LogWarning($"== == == == => animalMigrations {animalMigrations.Count}");

            var animalTypes = animalMigrations
                .SelectMany(migration => migration.Animal.AnimalTypes)
                .DistinctBy(animalType => animalType.Id)
                .ToList();

            _logger.LogWarning($"== == == == => animalTypes {animalTypes.Count}");

            var analytics = animalTypes.Select(animalType =>
            {
                var migrations = animalMigrations
                    .Where(migration => migration.Animal.AnimalTypes.Any(x => x.Id == animalType.Id))
                    .ToList();

                return new AnimalsAnalyticsDto(
                    AnimalType: animalType.Type,
                    AnimalTypeId: animalType.Id,
                    QuantityAnimals: migrations.Count(x => x.MigrationKind is AreaMigrationKind.Idle or AreaMigrationKind.Arrived),
                    AnimalsArrived: migrations.Count(x => x.MigrationKind is AreaMigrationKind.Arrived),
                    AnimalsGone: migrations.Count(x => x.MigrationKind is AreaMigrationKind.Gone));
            });

            _logger.LogWarning($"== == == == => analytics {analytics.Count()}");

            return new Response(
                TotalQuantityAnimals:
                    animalMigrations.Count(x => x.MigrationKind is AreaMigrationKind.Idle or AreaMigrationKind.Arrived) -
                    animalMigrations.Count(x => x.MigrationKind is AreaMigrationKind.Gone),
                TotalAnimalsArrived: animalMigrations.Count(x => x.MigrationKind is AreaMigrationKind.Arrived),
                TotalAnimalsGone: animalMigrations.Count(x => x.MigrationKind is AreaMigrationKind.Gone),
                AnimalsAnalytics: analytics);
        }

        /*public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var area =
            await _context.Areas.FindAsync(request.Id)
            ?? throw new NotFoundException();

        var totalVisits = await _context.AnimalLocationVisits
            .Include(visit => visit.Visitor.VisitedLocations)
            .Include(visit => visit.Visitor.AnimalTypes)
            .Include(visit => visit.LocationPoint)
            .Where(visit =>
                visit.DateTimeOfVisitLocationPoint >= request.StartDate &&
                visit.DateTimeOfVisitLocationPoint <= request.EndDate)
            .ToListAsync(cancellationToken);

        _logger.LogWarning("totalVisits:");
        foreach (var visit in totalVisits)
        {
            _logger.LogWarning(
                $"> {visit.Id}: {visit.DateTimeOfVisitLocationPoint} - {visit.LocationPoint.Longitude}, {visit.LocationPoint.Latitude}");
        }

        totalVisits = totalVisits
            .Where(visit => visit.LocationPoint.ToPoint().Overlays(area.ToPolygon()))
            .ToList();
        
        _logger.LogWarning("totalVisits:");
        foreach (var visit in totalVisits)
        {
            _logger.LogWarning(
                $"> {visit.Id}: {visit.DateTimeOfVisitLocationPoint} - {visit.LocationPoint.Longitude}, {visit.LocationPoint.Latitude}");
        }

        var visitsByAnimalType = totalVisits
            .SelectMany(visit => visit.Visitor.AnimalTypes)
            .DistinctBy(animalType => animalType.Id)
            .Select(animalType => new
            {
                AnimalType = animalType,
                Visits = totalVisits.Where(visit => visit.Visitor.AnimalTypes.Contains(animalType))
            });
        
        _logger.LogWarning("visitsByAnimalType:");
        foreach (var x in visitsByAnimalType)
        {
            _logger.LogWarning(
                $"> {x.AnimalType.Type}: {x.Visits.Count()}");
        }

        var analyticsByAnimalType = visitsByAnimalType
            .Select(pair => new AnimalsAnalyticsDto(
                pair.AnimalType.Type,
                pair.AnimalType.Id,
                pair.Visits.Count(),
                pair.Visits.Count(),
                pair.Visits.Count()))
            .ToList();

        return new Response(
            TotalQuantityAnimals: analyticsByAnimalType.Sum(x => x.QuantityAnimals),
            TotalAnimalsArrived: analyticsByAnimalType.Sum(x => x.AnimalsArrived),
            TotalAnimalsGone: analyticsByAnimalType.Sum(x => x.AnimalsGone),
            AnimalsAnalytics: analyticsByAnimalType);
    }*/
    }

    /*
    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var area =
            await _context.Areas.FindAsync(request.Id)
            ?? throw new NotFoundException();

        var polygon = area.ToPolygon();

        var visitorAnalytics =
            from animal in await _context.Animals.ToListAsync(cancellationToken)
            let totalVisits = animal.VisitedLocations.Where(visit =>
                visit.DateTimeOfVisitLocationPoint >= request.StartDate &&
                visit.DateTimeOfVisitLocationPoint <= request.EndDate)
            let areaVisitCount = totalVisits.Count(visit => visit.LocationPoint.ToPoint().Overlays(polygon))
            where areaVisitCount > 0
            select new
            {
                Visitor = animal,
                OnceLeft = totalVisits.Count() > areaVisitCount
            };

        var analyticsByAnimalType =
            from animalType in await _context.AnimalTypes.ToListAsync()
            let analytics = visitorAnalytics.Where(analytics => analytics.Visitor.AnimalTypes.Contains(animalType))
            select new AnimalsAnalyticsDto(
                animalType.Type,
                animalType.Id,
                analytics.Count(),
                analytics.Count(),
                analytics.Count(x => x.OnceLeft));

        var animalAnalytics = /*await#1# analyticsByAnimalType/*.ToListAsync(cancellationToken)#1#;
        return new Response(
            TotalQuantityAnimals: animalAnalytics.Count(),
            TotalAnimalsArrived: animalAnalytics.Count(),
            TotalAnimalsGone: animalAnalytics.Sum(x => x.AnimalsGone),
            AnimalsAnalytics: animalAnalytics);
    }
}
*/

    public record AnimalsAnalyticsDto(
        string AnimalType,
        long AnimalTypeId,
        int QuantityAnimals,
        int AnimalsArrived,
        int AnimalsGone);

    public sealed record Response(
        int TotalQuantityAnimals,
        int TotalAnimalsArrived,
        int TotalAnimalsGone,
        IEnumerable<AnimalsAnalyticsDto> AnimalsAnalytics);
}