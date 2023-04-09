using System.ComponentModel.DataAnnotations;
using DripChip.Domain.Constants;

#pragma warning disable CS8618
namespace DripChip.Domain.Entities;

public class Account : EntityBase<int>
{
    [MaxLength(25)] public string FirstName { get; set; }
    [MaxLength(25)] public string LastName { get; set; }

    public IList<Animal> ChippedAnimals { get; set; } = new List<Animal>();
}