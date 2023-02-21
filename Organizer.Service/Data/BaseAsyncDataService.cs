using Microsoft.EntityFrameworkCore.ChangeTracking;
using Organizer.Core.Interfaces.Data;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.Specification;
using Organizer.Core.Models;
using Organizer.Core.Specifications;

namespace Organizer.Service.Data
{
    //TODO: Error and Exception handling ?
    public class BaseAsyncDataService<T> : IBaseAsyncDataService<T> where T : BaseModel
    {
        private IUnitOfWork _unitOfWork;

        public BaseAsyncDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<T> AddAsync(T entity)
        {
            return await _unitOfWork.Repository<T>().AddAsync(entity);
        }

        public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            return _unitOfWork.Repository<T>().AddRangeAsync(entities);
        }

        public async Task<T> DeleteAsync(T entity)
        {
            return await _unitOfWork.Repository<T>().DeleteAsync(entity);
        }

        public async Task<IEnumerable<T>> DeleteRangeAsync(IEnumerable<T> entities)
        {
            return await _unitOfWork.Repository<T>().DeleteRangeAsync(entities);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _unitOfWork.Repository<T>().GetAllAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification)
        {
            return await _unitOfWork.Repository<T>().GetAllAsync(specification);
        }

        public async Task<T> GetEntityWithSpecificationAsync(ISpecification<T> specification)
        {
            return await _unitOfWork.Repository<T>().GetEntityWithSpecificationAsync(specification);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await _unitOfWork.Repository<T>().UpdateAsync(entity);
        }

        public async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities)
        {
            return await _unitOfWork.Repository<T>().UpdateRangeAsync(entities);
        }

        public async Task<int> SaveAllChangesAsync()
        {
           return await _unitOfWork.Complete();
        }
       
    }
}
