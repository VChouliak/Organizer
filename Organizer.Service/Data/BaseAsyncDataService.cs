using Organizer.Core.Interfaces.Data;
using Organizer.Core.Interfaces.Service;

namespace Organizer.Service.Data
{
    public class BaseAsyncDataService<T> : IBaseAsyncDataService<T> where T : class
    {
        private IAsyncRepository<T> _repository;

        public BaseAsyncDataService(IAsyncRepository<T> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        //TODO: Extend with other neccessary methods...
    }
}
