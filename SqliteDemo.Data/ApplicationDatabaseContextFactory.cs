using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SqliteDemo.Data.Design;

namespace SqliteDemo.Data;

public class ApplicationDatabaseContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
        string dbPath = Path.Combine(Environment.CurrentDirectory, DatabaseName);
        optionsBuilder.UseSqlite($"Filename={dbPath}");
        return new(optionsBuilder.Options, new CurrentDesignUserService { });
    }
}
