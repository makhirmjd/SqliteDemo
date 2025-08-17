using Microsoft.EntityFrameworkCore;
using SqliteDemo.Data;
using SqliteDemo.Data.Design;

DbContextOptionsBuilder<ApplicationDbContext> optionBuilder = new();
optionBuilder.UseSqlite($"Filename={DatabaseName}");

ApplicationDbContext context = new (optionBuilder.Options, new CurrentDesignUserService { });
context.Database.Migrate();

