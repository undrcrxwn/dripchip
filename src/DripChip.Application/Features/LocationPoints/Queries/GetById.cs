using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using FluentValidation;
using Mapster;
using Mediator;

namespace DripChip.Application.Features.LocationPoints.Queries;

public static class GetById
{
    public sealed record Query(long Id) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator() => RuleFor(x => x.Id).LocationPointId();
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity =
                await _context.LocationPoints.FindAsync(request.Id)
                ?? throw new NotFoundException(nameof(LocationPoint), request.Id);

            return entity.Adapt<Response>();
        }
    }

    public sealed record Response(long Id, double Latitude, double Longitude);
}