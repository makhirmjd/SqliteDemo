using SqliteDemo.Data;
using SqliteDemo.Models;

namespace SqliteDemo.Repositories;

public class CustomerRepository : ApplicationDbContext
{
    public CustomerRepository()
    {
        _ = CreateTable<Customer>();
    }

    public Task<List<Customer>> GetAllAsync() => GetAllAsync<Customer>();
}
