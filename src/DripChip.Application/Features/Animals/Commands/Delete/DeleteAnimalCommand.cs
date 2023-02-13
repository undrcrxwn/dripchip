using MediatR;

namespace DripChip.Application.Features.Animals.Commands.Delete;

public record DeleteAnimalCommand(long Id) : IRequest;