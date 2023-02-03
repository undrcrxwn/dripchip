using System.ComponentModel.DataAnnotations;

namespace DripChip.Application.DTOs;

public class RegisterAccountRequest
{
    [Required, Display(Name = "First name")]
    public required string FirstName { get; set; }
    
    [Required, Display(Name = "Last name")]
    public required string LastName { get; set; }
    
    [Required, EmailAddress, Display(Name = "Email")]
    public required string Email { get; set; }
    
    [Required, DataType(DataType.Password), Display(Name = "Password")]
    public required string Password { get; set; }
}