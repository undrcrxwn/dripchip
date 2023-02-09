using MediatR;

namespace DripChip.Application.Features.AnimalTypes.Queries.GetById;

public record GetAnimalTypeByIdQuery(long Id) : IRequest<GetAnimalTypeByIdResponse>;