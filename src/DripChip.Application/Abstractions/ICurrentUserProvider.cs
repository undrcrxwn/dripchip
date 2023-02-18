namespace DripChip.Application.Abstractions;

public interface ICurrentUserProvider
{
    public int? AccountId { get; }
    public bool IsAuthenticated { get; }
}