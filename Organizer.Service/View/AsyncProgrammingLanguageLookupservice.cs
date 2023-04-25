using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.Services;
using Organizer.Core.Models.Lookups;
using Organizer.Service.Data;

namespace Organizer.Service.View
{
    public class AsyncProgrammingLanguageLookupservice : IAsyncLookupService<LookupItem>
    {
        private readonly IProgrammingLanguageAsyncDataService _programmingLanguageAsyncDataService;

        public AsyncProgrammingLanguageLookupservice(IProgrammingLanguageAsyncDataService programmingLanguageAsyncDataService)
        {
            _programmingLanguageAsyncDataService = programmingLanguageAsyncDataService;
        }

        public async Task<IEnumerable<LookupItem>> GetLookupAsync()
        {
            var programmingLanguages = await _programmingLanguageAsyncDataService.GetAllAsync();

            if (programmingLanguages == null)
            {
                return Enumerable.Empty<LookupItem>();
            }

            return programmingLanguages.Select(programmingLanguage => new LookupItem
            {
                Id = programmingLanguage.Id,
                DisplayMember = programmingLanguage.Name
            }); ;
        }
    }
}
