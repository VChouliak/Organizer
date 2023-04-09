using Organizer.Core.Models.Entities;
using Organizer.Data.UnitOfWork;
using Organizer.Infrastructure.Data;

namespace Organizer.Service.Data
{
    internal class PhonesAsyncDataService : BaseAsyncDataService<FriendPhoneNumber>
    {
        public PhonesAsyncDataService() : base(new UnitOfWork(new OrganizerContext()))
        {
        }
    }
}
