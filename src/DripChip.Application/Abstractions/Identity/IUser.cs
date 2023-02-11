namespace DripChip.Application.Abstractions.Identity;

public interface IUser
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}