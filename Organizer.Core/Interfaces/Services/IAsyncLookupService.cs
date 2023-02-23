using Organizer.Core.Interfaces.Specification;
using Organizer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Core.Interfaces.Service
{
    public interface IAsyncLookupService<T>
    {
        public Task<IEnumerable<T>> GetLookupAsync();       
    }
}
