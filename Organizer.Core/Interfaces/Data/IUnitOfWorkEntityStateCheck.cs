using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Core.Interfaces.Data
{
    //TODO: adjust name
    public interface IUnitOfWorkEntityStateCheck
    {
        bool HasChanges();

        //TODO: extend with necessary methods
    }
}
