namespace SqliteDemo.Shared.Models;

public class Passport : Entity
{
    public DateTime ExpirationDate { get; set; }

    // Navigational Properties
    public Customer Customer { get; set; } = default!;
}
