namespace DripChip.Application.Abstractions;

/// <summary>
/// Abstraction declaring presentation-dependent user concerns.
/// </summary>
public interface ICurrentUserProvider
{
    public bool IsAuthenticated { get; }
    public int? AccountId { get; }
}