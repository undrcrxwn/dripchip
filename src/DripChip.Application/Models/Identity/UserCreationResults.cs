using DripChip.Application.Abstractions.Identity;
using DripChip.Domain.Entities;

namespace DripChip.Application.Models.Identity;

public abstract record UserCreationResult
{
    public record Success(Account Account) : UserCreationResult;
    public record Failure(IEnumerable<string> Reasons) : UserCreationResult;
}