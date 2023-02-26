using Organizer.Core.Interfaces.Service;
using Organizer.Core.Models.Entities;
using Organizer.Data;
using Organizer.Data.UnitOfWork;

namespace Organizer.Service.Data
{
    public class FriendsAsyncDataService : BaseAsyncDataService<Friend>, IFriendAsyncDataService
    {
        //TODO: Adjust constructor/s for unit of work and/or BaseDataService...?
        public FriendsAsyncDataService() : base(new UnitOfWork(new OrganizerContext()))
        {
        }
    }
}
