using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Core.Interfaces.Data
{
    public interface IUnitOfWorkBaseOperations
    {
        public Task<int> Complete();
        //TODO: extend with neccessary methods
    }
}
