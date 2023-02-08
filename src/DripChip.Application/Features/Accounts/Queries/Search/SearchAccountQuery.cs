using MediatR;

namespace DripChip.Application.Features.Accounts.Queries.Search;

public record SearchAccountQuery(
    string FirstName = "",
    string LastName = "",
    string Email = "",
    int From = 0,
    int Size = 10
) : IRequest<IEnumerable<SearchAccountResponse>>;