using DripChip.Application.Abstractions.Identity;

namespace DripChip.Application.Models.Identity;

public abstract record UserCreationResult
{
    public record Success(IUser User) : UserCreationResult;
    public record Failure(IEnumerable<string> Reasons) : UserCreationResult;
}