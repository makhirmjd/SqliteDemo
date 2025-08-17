using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqliteDemo.Shared.Models;

[Index(nameof(Name))]
[Index(nameof(Phone), IsUnique = true)]
public class Customer : Entity
{
    public int PassportId { get; set; }
    public string Name { get; set; } = default!;
    public string? Phone { get; set; }
    public int Age { get; set; }
    [MaxLength(100)]
    public string? Address { get; set; }

    // Navigational Properties
    public Passport? Passport { get; set; }
    public ICollection<Order> Orders { get; set; } = [];

    [NotMapped]
    public bool IsYoung => Age < 50;
}
