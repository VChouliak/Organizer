namespace Organizer.Core.Interfaces.Service
{
    public interface IBaseAsyncDataService<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();
    }
}
