namespace SqliteDemo.Models;

public class Customer
{
    public string Name { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public int Age { get; set; }
    public string Address { get; set; } = default!;
}
