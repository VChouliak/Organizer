using Microsoft.EntityFrameworkCore;
using Organizer.Core.Interfaces.Data;
using Organizer.Core.Interfaces.Specification;
using Organizer.Core.Models;

namespace Organizer.Data.Repository
{
    //TODO: Error and Exception handling, check and adjust return values... ?
    public class BaseAsyncRepository<T> : IBaseAsyncRepository<T> where T : BaseModel
    {
        private readonly DbContext _context;

        public BaseAsyncRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            var result = await _context.Set<T>().AddAsync(entity);
            return result.Entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            await Task.Run ( () => _context.Remove(entity));
            return entity;
        }

        public async Task<IEnumerable<T>> DeleteRangeAsync(IEnumerable<T> entities)
        {
            await Task.Run( () => _context.RemoveRange(entities));
            return entities;
        }

        public async Task<T> UpdateAsync(T entity)
        {
           var result = await Task.Run(() =>_context.Set<T>().Update(entity));
           return result.Entity;
        }

        public async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities)
        {
            await Task.Run(() => _context.Set<T>().UpdateRange());
            return entities;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluetor<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
        }

    }
}
