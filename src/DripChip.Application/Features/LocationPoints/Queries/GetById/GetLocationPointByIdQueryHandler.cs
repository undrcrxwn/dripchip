using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using Mapster;
using MediatR;

namespace DripChip.Application.Features.LocationPoints.Queries.GetById;

public class GetLocationPointByIdQueryHandler : IRequestHandler<GetLocationPointByIdQuery, GetLocationPointByIdResponse>
{
    private readonly IApplicationDbContext _context;

    public GetLocationPointByIdQueryHandler(IApplicationDbContext context) =>
        _context = context;
    
    public async Task<GetLocationPointByIdResponse> Handle(GetLocationPointByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.LocationPoints.FindAsync(request.Id);

        if (entity is null)
            throw new NotFoundException();
        
        return entity.Adapt<GetLocationPointByIdResponse>();
    }
}