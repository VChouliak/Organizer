using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Core.Interfaces.Data
{
    //TODO: Implement Specification - Pattern
    public interface IAsyncRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        //TODO: extend with other CRUD - Methods

    }
}
