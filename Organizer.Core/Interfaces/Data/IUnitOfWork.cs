using Organizer.Core.Models;

namespace Organizer.Core.Interfaces.Data
{
    public interface IUnitOfWork : IDisposable
    {
        public IBaseAsyncRepository<TModel> Repository<TModel>() where TModel : BaseModel;
        public Task<int> Complete();
    }
}
