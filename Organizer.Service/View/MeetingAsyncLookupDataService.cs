using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.Services;
using Organizer.Core.Models.Lookups;
using Organizer.Data.Specifications;
using Organizer.Service.Data;

namespace Organizer.Service.View
{
    public class MeetingAsyncLookupDataService : IAsyncLookupService<LookupItem>
    {
        private readonly IMeetingAsyncDataService _meetingDataService;

        public MeetingAsyncLookupDataService(IMeetingAsyncDataService meetingDataService)
        {
            _meetingDataService = meetingDataService;
        }

        public async Task<IEnumerable<LookupItem>> GetLookupAsync()
        {
            var meetings = await _meetingDataService.GetAllAsync(new MeetingsIncludeFriendsSpecification());

            if (meetings == null)
            {
                return Enumerable.Empty<LookupItem>();
            }

            return meetings.Select(programmingLanguage => new LookupItem
            {
                Id = programmingLanguage.Id,
                DisplayMember = programmingLanguage.Title
            }); 
        }
    }
}
