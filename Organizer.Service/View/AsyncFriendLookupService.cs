using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.Specification;
using Organizer.Core.Models;
using Organizer.Core.Specifications;
using Organizer.Service.Data;

namespace Organizer.Service.View
{
    public class AsyncFriendLookupService : IAsyncLookupService<LookupItem>
    {
        private readonly FriendsAsyncDataService _friendsAsyncDataService;

        public AsyncFriendLookupService(FriendsAsyncDataService friendsAsyncDataService)
        {
            _friendsAsyncDataService = friendsAsyncDataService;
        }

        public async Task<IEnumerable<LookupItem>> GetLookupAsync()
        {
            var friends = await _friendsAsyncDataService.GetAllAsync(new FriendsOrderedByFirstNameSpecification());

            if (friends == null)
            {
                return Enumerable.Empty<LookupItem>();
            }

            return friends.Select(friend => new LookupItem
            {
                Id = friend.Id,
                DisplayMember = $"{friend.FirstName} {friend.LastName}"
            });

        }
    }

}
