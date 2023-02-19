using Organizer.Core.Interfaces.Service;
using Organizer.Core.Models;
using Organizer.Data.Repository;

namespace Organizer.Service.Data
{
    public class FriendsAsyncDataService :  BaseAsyncDataService<Friend>, IFriendAsyncDataService
    {
        public FriendsAsyncDataService() : base(new FriendsAsyncRepository())
        {

        }      
    }
}
