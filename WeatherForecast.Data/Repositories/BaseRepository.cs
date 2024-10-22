using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data.DbContext;
using WeatherForecast.Data.Entities;
using WeatherForecast.Shared.Exceptions;

namespace WeatherForecast.Data.Repositories;

public abstract class BaseRepository<T> where T : Entity
{
    protected readonly WeatherForecastDbContext _context;

    protected BaseRepository(WeatherForecastDbContext context)
    {
        _context = context;
    }

    protected IQueryable<T> GetEntitiesQuery() =>  _context.Set<T>();

    protected async Task<T> GetEntityByIdAsync(int id)
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        ThrowIfNotExists(entity, id);
        return entity!;
    }

    protected async Task<int> AddEntityAsync(T entity)
    {
        var nextId = (await _context.Set<T>().MaxAsync(x => (int?)x.Id) ?? 0) + 1;
        entity.Id = nextId;
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    protected async Task<int> DeleteEntityAsync(int id)
    {
        var entity = await GetEntityByIdAsync(id);
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    protected void ThrowIfNotExists(object? obj, int id)
    {
        if (obj == default)
            throw new EntityNotFoundException($"Not found entity with id:{id}");
    }
}