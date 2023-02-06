namespace DripChip.Application.DTOs;

public class SearchAccountRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int From { get; set; } = 0;
    public int Size { get; set; } = 10;
}