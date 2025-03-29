using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRN222_TaskManagement.Models;
using PRN222_TaskManagement.Services;

public class BaseServiceImplement<T, TKey> : IBaseService<T, TKey> where T : class
{
    protected readonly Prn222TaskManagementContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly ILogger<BaseServiceImplement<T, TKey>> _logger;

    public BaseServiceImplement(Prn222TaskManagementContext context, ILogger<BaseServiceImplement<T, TKey>> logger)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _logger = logger;
    }

    // ✅ Get All
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        _logger.LogInformation($"[{typeof(T).Name}] Fetching all records.");
        return await _dbSet.ToListAsync();
    }

    // Get By Id
    public virtual async Task<T> GetByIdAsync(TKey id)
    {
        _logger.LogInformation($"[{typeof(T).Name}] Fetching record with ID: {id}");
        return await _dbSet.FindAsync(id);
    }

    // Get By Condition
    public virtual async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
    {
        _logger.LogInformation($"[{typeof(T).Name}] Fetching records by condition.");
        return await _dbSet.Where(predicate).ToListAsync();
    }

    // Add
    public virtual async Task<T> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"[{typeof(T).Name}] Added new entity: {entity}");
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[{typeof(T).Name}] Error while adding entity.");
            throw;
        }
    }

    // Update
    public virtual async Task<T> UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"[{typeof(T).Name}] Updated entity: {entity}");
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[{typeof(T).Name}] Error while updating entity.");
            throw;
        }
    }

    // Delete
    public virtual async Task<bool> DeleteAsync(TKey id)
    {
        try
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                _logger.LogWarning($"[{typeof(T).Name}] Attempt to delete non-existing entity with ID: {id}");
                return false;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"[{typeof(T).Name}] Deleted entity with ID: {id}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[{typeof(T).Name}] Error while deleting entity with ID: {id}");
            throw;
        }
    }
}
