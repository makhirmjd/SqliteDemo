using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using SqliteDemo.Shared.Models;
using SqliteDemo.Shared.Models.Annotations;
using SqliteDemo.Shared.Services;
using System.Reflection;

namespace SqliteDemo.Data;

public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Passport> Passports => Set<Passport>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        ICollection<IMutableEntityType> entityTypes = [..modelBuilder.Model.GetEntityTypes().Where(e => !e.IsOwned())];
        foreach (IMutableEntityType entityType in entityTypes)
        {
            ICollection<IMutableProperty> properties = [..entityType.GetProperties()];
            foreach (IMutableProperty? property in properties)
            {
                if (property.Name.Equals(Created, StringComparison.InvariantCultureIgnoreCase) ||
                    property.Name.Equals(LastModified, StringComparison.InvariantCultureIgnoreCase))
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTimeOffset>(property.Name).HasDefaultValueSql("getutcdate()");
                }

                if (property.ClrType == typeof(string) && property.Name.EndsWith("Url", StringComparison.InvariantCultureIgnoreCase))
                {
                    property.SetIsUnicode(false);
                }

                if (property.IsForeignKey() && 
                    property.PropertyInfo?.GetCustomAttribute(typeof(DeleteActionAttribute)) is DeleteActionAttribute deleteActionAttribute)
                {
                    IMutableForeignKey? foreignKey = property.GetContainingForeignKeys().FirstOrDefault();
                    if (foreignKey is not null)
                    {
                        foreignKey.DeleteBehavior = deleteActionAttribute.DeleteBehavior;
                    }
                }

                if (property.IsForeignKey() &&
                    property.PropertyInfo?.GetCustomAttribute(typeof(DefaultPropertyValueAttribute)) is DefaultPropertyValueAttribute defaultPropertyValueAttribute)
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name)
                        .ValueGeneratedNever()
                        .HasDefaultValue(defaultPropertyValueAttribute.DefaultValue);
                }
            }
        }
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        Task task = AssignStamp();
        task.ContinueWith(_ => base.SaveChanges(acceptAllChangesOnSuccess));
        return 0;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        await AssignStamp();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private async Task AssignStamp()
    {
        string? user = await currentUserService.GetUserId() ?? "system";
        DateTimeOffset timeStamp = DateTimeOffset.UtcNow;
        ICollection<EntityEntry> entries = 
            [..ChangeTracker.Entries().Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) && !e.Metadata.IsOwned())];
        foreach (EntityEntry entry in entries)
        {
            entry.Property(LastModified).CurrentValue = timeStamp;
            entry.Property(LastModifiedBy).CurrentValue = user;
            if (entry.State == EntityState.Added)
            {
                entry.Property(Created).CurrentValue = timeStamp;
                entry.Property(CreatedBy).CurrentValue = user;
            }
        }
    }
}
