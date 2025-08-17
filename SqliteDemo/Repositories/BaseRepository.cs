using Microsoft.EntityFrameworkCore;
using SqliteDemo.Data;
using SqliteDemo.Shared.Models;
using System.Linq.Expressions;

namespace SqliteDemo.Repositories;

public partial class BaseRepository<T>(ApplicationDbContext context) : IDisposable where T : Entity, new()
{
    public (bool IsSuccess, int RowsAffected, string StatusMessage) LastOperationStatus { get; set; }

    public async Task AddAsync(T entity)
    {
        try
        {
            await context.Set<T>().AddAsync(entity);
            int rowsAffected = await context.SaveChangesAsync();
            LastOperationStatus = (true, rowsAffected, $"{typeof(T).Name} added successfully.");
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error adding {typeof(T).Name}: {ex.Message}");
        }
    }

    public async Task<List<T>> GetAllAsync()
    {
        try
        {
            List<T> entities = await context.Set<T>().ToListAsync();
            LastOperationStatus = (true, entities.Count, $"{typeof(T).Name}s retrieved successfully.");
            return entities;
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error retrieving {typeof(T).Name}s: {ex.Message}");
            return [];
        }
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression)
    {
        try
        {
            List<T> entities = await context.Set<T>().Where(expression).ToListAsync();
            LastOperationStatus = (true, entities.Count, $"{typeof(T).Name}s retrieved successfully.");
            return entities;
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error retrieving {typeof(T).Name}s : {ex.Message}");
            return [];
        }
    }

    public async Task<List<T>> GetAllAsync(string query)
    {
        try
        {
            List<T> entities = await context.Set<T>().FromSqlRaw(query).ToListAsync();
            LastOperationStatus = (true, entities.Count, $"{typeof(T).Name}s retrieved successfully.");
            return entities;
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error executing query for {typeof(T).Name}s: {ex.Message}");
            return [];
        }
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        try
        {
            T? entity = await context.Set<T>().FindAsync(id);
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

    public async Task UpdateAsync(T entity)
    {
        try
        {
            context.Set<T>().Update(entity);
            int rowsAffected = await context.SaveChangesAsync();
            LastOperationStatus = (true, rowsAffected, $"{typeof(T).Name} updated successfully.");
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error updating {typeof(T).Name}: {ex.Message}");
        }
    }

    public async Task DeleteAsync(T entity)
    {
        try
        {
            context.Remove(entity);
            int rowsAffected = await context.SaveChangesAsync();
            LastOperationStatus = (true, rowsAffected, $"{typeof(T).Name} deleted successfully.");
        }
        catch (Exception ex)
        {
            LastOperationStatus = (false, 0, $"Error deleting {typeof(T).Name}: {ex.Message}");
        }
    }

    public async Task DeleteAsync(object id)
    {
        try
        {
            T? entity = await GetByIdAsync(id);
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

    public async void Dispose()
    {
        await context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
