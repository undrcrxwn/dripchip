using MediatR;

namespace DripChip.Application.Features.Animals.Queries.GetById;

public record GetAnimalByIdQuery(long Id) : IRequest<GetAnimalByIdResponse>;