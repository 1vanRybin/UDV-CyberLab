using Domain.Interfaces;
using ExampleCore.Dal.Base;
using Medo;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Data;

public class BaseRepository: IStandartStore
{
    private readonly DbContext _applicationDbContext;
    public BaseRepository(DbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<T?> GetByIdAsync<T>(Guid id, bool asNoTracking = false)
        where T : BaseEntity<Guid>
    {
        IQueryable<T>  query = _applicationDbContext.Set<T>();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        var res = await query.FirstOrDefaultAsync(e => e.Id == id);

        return res;
    }

    public async Task<Guid> CreateAsync<T>(T entity)
        where T : BaseEntity<Guid>
    {
        var entityId = new Uuid7().ToGuid();
        entity.Id = entityId;
        await _applicationDbContext.Set<T>().AddAsync(entity);
        await _applicationDbContext.SaveChangesAsync();

        return entityId;
    }

    public async Task<Guid> CreateAsync<T>(Guid id, T entity)
        where T : BaseEntity<Guid>
    {
        entity.Id = id;
        await _applicationDbContext.Set<T>().AddAsync(entity);
        await _applicationDbContext.SaveChangesAsync();

        return id;
    }
    public async Task<T> UpdateAsync<T>(T entity)
        where T : BaseEntity<Guid>
    {
        _applicationDbContext.Set<T>().Update(entity);

        await _applicationDbContext.SaveChangesAsync();

        return entity;
    }


    public async Task<bool> DeleteAsync<T>(T entity)
        where T : BaseEntity<Guid>
    {
        _applicationDbContext.Set<T>().Remove(entity);

        var affectedRows  = await _applicationDbContext.SaveChangesAsync();

        return affectedRows > 0;
    }

    public async Task<List<T>> GetAllAsync<T>()
        where T : BaseEntity<Guid>
    {
        var res = await _applicationDbContext.Set<T>().ToListAsync();

        return res;
    }

    public async Task<List<T>> GetPaginatedAsync<T>(int page, int pageSize)
        where T : BaseEntity<Guid>
    {
        var paginatedList = await _applicationDbContext.Set<T>()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return paginatedList;
    }
}