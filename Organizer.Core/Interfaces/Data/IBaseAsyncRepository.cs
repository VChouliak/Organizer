﻿using Organizer.Core.Interfaces.Specification;

namespace Organizer.Core.Interfaces.Data
{//TODO: Split interface
    public interface IBaseAsyncRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification);
        Task<T> GetEntityWithSpecificationAsync(ISpecification<T> specification);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> DeleteRangeAsync(IEnumerable<T> entities);
       

    }
}
