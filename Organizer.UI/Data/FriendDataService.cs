using Organizer.Core.Interfaces;
using Organizer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    internal class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            yield return new Friend { FirstName = "Thomas", LastName = "Huber" };
            yield return new Friend { FirstName = "Andreas", LastName = "Boehler" };
            yield return new Friend { FirstName = "Julia", LastName = "Huber" };
            yield return new Friend { FirstName = "Chrissi", LastName = "Egin" };
        }
    }
}
