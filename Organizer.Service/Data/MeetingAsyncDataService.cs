using Organizer.Core.Interfaces.Services;
using Organizer.Core.Models.Entities;
using Organizer.Data.UnitOfWork;
using Organizer.Infrastructure.Data;

namespace Organizer.Service.Data
{
    public class MeetingAsyncDataService : BaseAsyncDataService<Meeting>, IMeetingAsyncDataService
    {
        public MeetingAsyncDataService(): base(new UnitOfWork(new OrganizerContext()))
        {
        }
    }
}
