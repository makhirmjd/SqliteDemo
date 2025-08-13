using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SqliteDemo.Models;

[Table("Customers")]
public class Customer : Entity
{
    [Indexed, NotNull]
    public string Name { get; set; } = default!;
    [Unique]
    public string Phone { get; set; } = default!;
    public int Age { get; set; }
    [MaxLength(100)]
    public string Address { get; set; } = default!;

    [Ignore]
    public bool IsYoung => Age < 50;

    [ForeignKey(typeof(Passport))]
    public int PassportId { get; set; }
    [OneToOne(CascadeOperations = CascadeOperation.All)]
    public Passport? Passport { get; set; }
}
