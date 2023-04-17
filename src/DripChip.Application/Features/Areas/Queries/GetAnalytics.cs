using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Geo;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

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

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var area =
                await _context.Areas.FindAsync(request.Id)
                ?? throw new NotFoundException();

            var polygon = new Polygon
            {
                Points = area.AreaPoints.ToArray<Point>()
            };

            var visitorAnalytics =
                from animal in _context.Animals
                let totalVisits = animal.VisitedLocations.Where(visit =>
                    visit.DateTimeOfVisitLocationPoint >= request.StartDate &&
                    visit.DateTimeOfVisitLocationPoint <= request.EndDate)
                let areaVisitCount = totalVisits.Count(visit => visit.LocationPoint.Overlaps(polygon))
                where areaVisitCount > 0
                select new
                {
                    Visitor = animal,
                    OnceLeft = totalVisits.Count() > areaVisitCount
                };

            var analyticsByAnimalType =
                from animalType in _context.AnimalTypes
                let analytics = visitorAnalytics.Where(analytics => analytics.Visitor.AnimalTypes.Contains(animalType))
                select new AnimalsAnalyticsDto(
                    animalType.Type,
                    animalType.Id,
                    analytics.Count(),
                    analytics.Count(),
                    analytics.Count(x => x.OnceLeft));

            var animalAnalytics = await analyticsByAnimalType.ToListAsync(cancellationToken);
            return new Response(
                TotalQuantityAnimals: animalAnalytics.Count,
                TotalAnimalsArrived: animalAnalytics.Count,
                TotalAnimalsGone: animalAnalytics.Sum(x => x.AnimalsGone),
                AnimalsAnalytics: animalAnalytics);
        }
    }

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