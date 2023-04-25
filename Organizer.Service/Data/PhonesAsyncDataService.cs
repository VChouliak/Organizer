using Organizer.Core.Interfaces.Data;
using Organizer.Core.Models.Entities;
using Organizer.Data.UnitOfWork;
using Organizer.Infrastructure.Data;

namespace Organizer.Service.Data
{
    internal class PhonesAsyncDataService : BaseAsyncDataService<FriendPhoneNumber>
    {
        public PhonesAsyncDataService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
