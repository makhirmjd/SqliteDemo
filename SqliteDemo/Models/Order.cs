namespace SqliteDemo.Models;

public class Order : Entity
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
}
