using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Constants;
using DripChip.Domain.Entities;
using DripChip.Spatial;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Features.Areas.Commands;

public static class Create
{
    public sealed record AreaPointDto(double Longitude, double Latitude);

    public sealed record Command(string Name, IEnumerable<AreaPointDto> AreaPoints) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.AreaPoints.Count()).GreaterThanOrEqualTo(3);
            RuleForEach(x => x.AreaPoints).NotNull().ChildRules(builder =>
            {
                builder.RuleFor(x => x.Latitude).Latitude();
                builder.RuleFor(x => x.Longitude).Longitude();
            });

            RuleFor(x => x.AreaPoints).Custom((areaPoints, context) =>
            {
                var points = areaPoints.Select(areaPoint => new Point(areaPoint.Longitude, areaPoint.Latitude));

                var polygon = new Polygon(points.ToArray());
                if (polygon.HasIntersections())
                    context.AddFailure(context.PropertyName, "Polygon is not allowed to have intersections.");
            });
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserProvider _issuer;

        public Handler(IApplicationDbContext context, ICurrentUserProvider issuer)
        {
            _context = context;
            _issuer = issuer;
        }

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var issuer = await _issuer.GetAccountAsync();
            if (issuer?.Role != Roles.Admin)
                throw new ForbiddenException();

            var area = new Area { Name = request.Name };

            area.AreaPoints = request.AreaPoints.Select((point, i) => new AreaPoint
            {
                Longitude = point.Longitude,
                Latitude = point.Latitude,
                Area = area,
                SequenceId = i
            }).ToList();

            var existingAreaPoints = await _context.Areas
                .Select(are => are.AreaPoints.Select(point => point.ToPoint()))
                .ToListAsync(cancellationToken);
            
            var existingPolygons = existingAreaPoints.Select(areaPoints => new Polygon(areaPoints.ToArray()));

            var polygon = area.ToPolygon();

            if (existingPolygons.Any(polygon.Overlays))
                throw new ValidationException();

            await _context.Areas.AddAsync(area, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return area.Adapt<Response>();
        }
    }

    public sealed record Response(long Id, string Name, IEnumerable<AreaPointDto> AreaPoints);
}