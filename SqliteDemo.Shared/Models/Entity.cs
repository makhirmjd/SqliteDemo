namespace SqliteDemo.Shared.Models;

public class Entity
{
    public int Id { get; set; }
    public string? CreatedBy { get; set; } = Anonymous;
    public DateTimeOffset Created { get; set; }
    public string? LastModifiedBy { get; set; } = Anonymous;
    public DateTimeOffset LastModified { get; set; }
}
