using MediatR;

namespace DripChip.Application.Features.Accounts.Commands.Delete;

public record DeleteAccountCommand(int AccountId) : IRequest;