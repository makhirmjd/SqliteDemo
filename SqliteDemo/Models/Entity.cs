using SQLite;

namespace SqliteDemo.Models;

public class Entity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
}
