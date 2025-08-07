using SQLite;

namespace SqliteDemo.Helpers;

public static class Constants
{
    private const string DatabaseName = "SqliteDemo.db3";

    public const SQLiteOpenFlags Flags =
        SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
}
