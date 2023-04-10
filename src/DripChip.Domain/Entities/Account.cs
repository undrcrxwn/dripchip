using DripChip.Domain.Constants;
using Microsoft.AspNetCore.Identity;

#pragma warning disable CS8618
namespace DripChip.Domain.Entities;

public class Account : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    private string _role = Roles.User;

    public string Role
    {
        get => _role;
        set => _role = Roles.Contain(value)
            ? value
            : throw new ArgumentException(nameof(value), "The specified role is invalid.");
    }

    public IList<Animal> ChippedAnimals { get; set; } = new List<Animal>();
}