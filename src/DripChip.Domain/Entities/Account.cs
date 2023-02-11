namespace DripChip.Domain.Entities;

public class Account : EntityBase<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}