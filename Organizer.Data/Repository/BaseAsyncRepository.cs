using Microsoft.EntityFrameworkCore;
using Organizer.Core.Interfaces.Data;

namespace Organizer.Data.Repository
{
    //TODO: Implement Specification - Pattern
    public class BaseAsyncRepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly DbContext _context;

        public BaseAsyncRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        //TODO: extend with other CRUD - Methods
    }
}
