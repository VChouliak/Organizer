using Organizer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Core.Interfaces
{
    public interface IFriendDataService
    {
        public IEnumerable<Friend> GetAll();
    }
}
