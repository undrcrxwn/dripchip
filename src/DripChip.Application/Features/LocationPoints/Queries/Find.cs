using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.LocationPoints.Queries;

public static class Find
{
    public sealed record Query(double Longitude, double Latitude) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Longitude).Longitude();
            RuleFor(x => x.Latitude).Latitude();
        }
    }

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var point =
                await _context.LocationPoints.FirstOrDefaultAsync(point =>
                    point.Longitude == request.Longitude &&
                    point.Latitude == request.Latitude, cancellationToken)
                ?? throw new NotFoundException();

            return new Response(point.Id);
        }
    }

    public sealed record Response(long Id);
}