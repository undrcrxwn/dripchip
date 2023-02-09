using MediatR;

namespace DripChip.Application.Features.Accounts.Queries.Search;

public record SearchAccountQuery(
    string? FirstName = default,
    string? LastName = default,
    string? Email = default,
    int From = 0,
    int Size = 10
) : IRequest<IEnumerable<SearchAccountResponse>>;