using SQLite;
using SqliteDemo.Models;
using System.Linq.Expressions;

namespace SqliteDemo.Data;

public partial class ApplicationDbContext
{
    private readonly SQLiteAsyncConnection connection;
    public (bool IsSuccess, int RowsAffected, string StatusMessage) LastOperationStatus { get; set; }

    public ApplicationDbContext()
    {
        connection = new SQLiteAsyncConnection(DatabasePath, Flags);
    }

    public async Task CreateTable<T>() where T : Entity, new()
    {
        try
        {
            await connection.CreateTableAsync<T>();
            LastOperationStatus = (true, 0, $"{typeof(T).Name} table created successfully.");
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error creating {typeof(T).Name} table: {ex.Message}");
        }
    }

    public async Task AddAsync<T>(T entity) where T : Entity, new()
    {
        try
        {
            int rowsAffected = await connection.InsertAsync(entity);
            LastOperationStatus = (true, rowsAffected, $"{typeof(T).Name} added successfully.");
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error adding {typeof(T).Name}: {ex.Message}");
        }
    }

    public async Task<List<T>> GetAllAsync<T>() where T : Entity, new()
    {
        try
        {
            List<T> entities = await connection.Table<T>().ToListAsync();
            LastOperationStatus = (true, entities.Count, $"{typeof(T).Name}s retrieved successfully.");
            return entities;
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error retrieving {typeof(T).Name}s: {ex.Message}");
            return [];
        }
    }

    public async Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> expression) where T : Entity, new()
    {
        try
        {
            List<T> entities = await connection.Table<T>().Where(expression).ToListAsync();
            LastOperationStatus = (true, entities.Count, $"{typeof(T).Name}s retrieved successfully.");
            return entities;
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error retrieving {typeof(T).Name}s : {ex.Message}");
            return [];
        }
    }

    public async Task<List<T>> GetAllAsync<T>(string query) where T : Entity, new()
    {
        try
        {
            List<T> entities = await connection.QueryAsync<T>(query);
            LastOperationStatus = (true, entities.Count, $"{typeof(T).Name}s retrieved successfully.");
            return entities;
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error executing query for {typeof(T).Name}s: {ex.Message}");
            return [];
        }
    }

    public async Task<T?> GetByIdAsync<T>(int id) where T : Entity, new()
    {
        try
        {
            T? entity = await connection.FindAsync<T>(id);
            if (entity != null)
            {
                LastOperationStatus = (true, 1, $"{typeof(T).Name} with ID {id} retrieved successfully.");
            }
            else
            {
                LastOperationStatus = (false, 0, $"{typeof(T).Name} with ID {id} not found.");
            }
            return entity;
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error retrieving {typeof(T).Name} with ID {id}: {ex.Message}");
            return default;
        }
    }

    public async Task UpdateAsync<T>(T entity) where T : Entity, new()
    {
        try
        {
            int rowsAffected = await connection.UpdateAsync(entity);
            LastOperationStatus = (true, rowsAffected, $"{typeof(T).Name} updated successfully.");
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error updating {typeof(T).Name}: {ex.Message}");
        }
    }

    public async Task DeleteAsync<T>(T entity) where T : Entity, new()
    {
        try
        {
            int rowsAffected = await connection.DeleteAsync(entity);
            LastOperationStatus = (true, rowsAffected, $"{typeof(T).Name} deleted successfully.");
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error deleting {typeof(T).Name}: {ex.Message}");
        }
    }

    public async Task DeleteAsync<T>(int id) where T : Entity, new()
    {
        try
        {
            T? entity = await GetByIdAsync<T>(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
            else
            {
                LastOperationStatus = (false, 0, $"{typeof(T).Name} with ID {id} not found.");
            }
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error deleting {typeof(T).Name} with ID {id}: {ex.Message}");
        }
    }
}
