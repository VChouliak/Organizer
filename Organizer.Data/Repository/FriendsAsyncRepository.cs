using Organizer.Core.Models;

namespace Organizer.Data.Repository
{
    public class FriendsAsyncRepository : BaseAsyncRepository<Friend>
    {
        public FriendsAsyncRepository() : base(new OrganizerContext())
        {
        }
    }
}
