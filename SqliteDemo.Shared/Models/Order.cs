namespace SqliteDemo.Shared.Models;

public class Order : Entity
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }

    // Navigational Properties
    public Customer? Customer { get; set; }
}
