using SQLite;

namespace SqliteDemo.Models;

[Table("Customers")]
public class Customer
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Indexed, NotNull]
    public string Name { get; set; } = default!;
    [Unique]
    public string Phone { get; set; } = default!;
    public int Age { get; set; }
    [MaxLength(100)]
    public string Address { get; set; } = default!;

    [Ignore]
    public bool IsYoung => Age < 50;
}
