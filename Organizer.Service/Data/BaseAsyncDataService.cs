using Microsoft.EntityFrameworkCore.ChangeTracking;
using Organizer.Core.Interfaces.Data;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.Specification;
using Organizer.Core.Models;
using Organizer.Core.Specifications;

namespace Organizer.Service.Data
{
    //TODO: Error and Exception handling ?
    public abstract class BaseAsyncDataService<T> : IBaseAsyncDataService<T> where T : BaseModel
    {
        private IUnitOfWork _unitOfWork;

        public BaseAsyncDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            return await _unitOfWork.Repository<T>().AddAsync(entity);
        }

        public virtual Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            return _unitOfWork.Repository<T>().AddRangeAsync(entities);
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            return await _unitOfWork.Repository<T>().DeleteAsync(entity);
        }

        public virtual async Task<IEnumerable<T>> DeleteRangeAsync(IEnumerable<T> entities)
        {
            return await _unitOfWork.Repository<T>().DeleteRangeAsync(entities);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _unitOfWork.Repository<T>().GetAllAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification)
        {
            return await _unitOfWork.Repository<T>().GetAllAsync(specification);
        }

        public virtual async Task<T> GetEntityWithSpecificationAsync(ISpecification<T> specification)
        {
            return await _unitOfWork.Repository<T>().GetEntityWithSpecificationAsync(specification);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            return await _unitOfWork.Repository<T>().UpdateAsync(entity);
        }

        public virtual async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities)
        {
            return await _unitOfWork.Repository<T>().UpdateRangeAsync(entities);
        }

        public virtual async Task<int> SaveAllChangesAsync()
        {
           return await _unitOfWork.Complete();
        }
       
    }
}
