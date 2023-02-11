#pragma warning disable CS8618
namespace DripChip.Domain.Entities;

public class Account : EntityBase<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}