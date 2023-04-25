using Organizer.Core.Interfaces.Data;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Models.Entities;
using Organizer.Data;
using Organizer.Data.UnitOfWork;
using Organizer.Infrastructure.Data;

namespace Organizer.Service.Data
{
    public class FriendsAsyncDataService : BaseAsyncDataService<Friend>, IFriendsAsyncDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        //TODO: Adjust constructor/s for unit of work and/or BaseDataService...?
        public FriendsAsyncDataService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ReloadFriendAsync(int id)
        {
            var context = ((UnitOfWork) _unitOfWork).GetDbContext(); //TODO: adjust or refactor Interface
            var dbEntry = context.ChangeTracker.Entries<Friend>().SingleOrDefault(db=>db.Entity.Id == id);
            if (dbEntry != null)
            {
                await dbEntry.ReloadAsync();
            }
        }
    }
}
