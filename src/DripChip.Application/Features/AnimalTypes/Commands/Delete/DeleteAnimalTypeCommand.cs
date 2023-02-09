using MediatR;

namespace DripChip.Application.Features.AnimalTypes.Commands.Delete;

public record DeleteAnimalTypeCommand(long Id) : IRequest;