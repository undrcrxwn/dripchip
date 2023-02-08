using MediatR;

namespace DripChip.Application.Features.Accounts.Queries.GetById;

public record GetAccountByIdQuery(int AccountId) : IRequest<GetAccountByIdResponse>;