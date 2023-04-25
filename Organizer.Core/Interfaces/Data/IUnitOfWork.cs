using Organizer.Core.Models;

namespace Organizer.Core.Interfaces.Data
{
    public interface IUnitOfWork : IUnitOfWorkBaseOperations, IUnitOfWorkEntityStateCheck, IDisposable
    {
        public IBaseAsyncRepository<TModel> Repository<TModel>() where TModel : BaseModel;       
    }
}
