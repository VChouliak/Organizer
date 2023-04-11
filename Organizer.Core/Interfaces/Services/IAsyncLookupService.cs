namespace Organizer.Core.Interfaces.Service
{
    public interface IAsyncLookupService<T>
    {
        public Task<IEnumerable<T>> GetLookupAsync();       
    }
}
