namespace DripChip.Application.Abstractions.Identity;

/// <summary>
/// User abstraction declaring infrastructure-dependent identity concerns.
/// </summary>
public interface IUser
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}