﻿using ExampleCore.Dal.Base;

namespace Domain.Interfaces;

public interface IStandartStore
{
    Task<T?> GetByIdAsync<T>(Guid id, bool asNoTracking = false) where T : BaseEntity<Guid>;
    Task<Guid> CreateAsync<T>(T entity) where T : BaseEntity<Guid>;
    Task<Guid> CreateAsync<T>(Guid id, T entity) where T : BaseEntity<Guid>;
    Task<T> UpdateAsync<T>(T entity) where T : BaseEntity<Guid>;
    Task<bool> DeleteAsync<T>(T entity) where T : BaseEntity<Guid>;
    Task<List<T>> GetAllAsync<T>() where T : BaseEntity<Guid>;
    Task<List<T>> GetPaginatedAsync<T>(int page, int pageSize) where T : BaseEntity<Guid>;
}