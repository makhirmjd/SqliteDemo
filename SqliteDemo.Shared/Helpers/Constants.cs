using SqliteDemo.Shared.Models;
using System.Reflection;

namespace SqliteDemo.Shared.Helpers;

public static class Constants
{
    // Db Configuration
    public const string DatabaseName = "SqliteDemo.db3";
    public const string Admin = "Admin";
    public const string Created = "Created";
    public const string CreatedBy = "CreatedBy";
    public const string LastModified = "LastModified";
    public const string LastModifiedBy = "LastModifiedBy";
    public const string Anonymous = "Anonymous";

    // Mapper Configuration
    public static U MapLite<T, U>(T src, U dest) where U : class, new()
    {
        List<PropertyInfo> properties = [.. typeof(U).GetProperties()];
        typeof(T).GetProperties().Where(x => !typeof(Entity).GetProperties().Any(y => y.Name == x.Name) && (x.PropertyType == typeof(string) || x.PropertyType.IsValueType)).ToList().ForEach(x =>
        {
            PropertyInfo? property = properties.FirstOrDefault(p => p.Name == x.Name);
            if (property is not null && property.CanWrite && property.GetType() == x.GetType())
            {
                property.SetValue(dest, x.GetValue(src), null);
            }
        });
        return dest;
    }

    public static U? MapLites<T, U>(T? src, U? dest) where U : class, new()
    {
        List<PropertyInfo> properties = [.. typeof(U).GetProperties()];
        typeof(T).GetProperties().Where(x => !typeof(Entity).GetProperties().Any(y => y.Name == x.Name) && (x.PropertyType == typeof(string) || x.PropertyType.IsValueType)).ToList().ForEach(x =>
        {
            PropertyInfo? property = properties.FirstOrDefault(p => p.Name == x.Name);
            if (property is not null && property.CanWrite && property.GetType() == x.GetType())
            {
                property.SetValue(dest, x.GetValue(src), null);
            }
        });
        return dest;
    }

    public static List<U> MapLite<T, U>(List<T> srcs) where U : class, new() => srcs.Select(s => MapLite(s, new U { })).ToList();

    public static List<T> MapLite<T>(List<T> srcs) where T : class, new() => srcs.Select(s => MapLite(s, new T { })).ToList();
}
