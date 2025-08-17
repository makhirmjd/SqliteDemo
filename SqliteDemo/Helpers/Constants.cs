namespace SqliteDemo.Helpers;

public static class Constants
{
    // Db Configuration
    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseName);
}
