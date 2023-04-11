using Organizer.Core.Interfaces.Services;
using Organizer.Core.Models.Entities;
using Organizer.Data.UnitOfWork;
using Organizer.Infrastructure.Data;

namespace Organizer.Service.Data
{
    public class ProgrammingLanguagesAsyncDataService : BaseAsyncDataService<ProgrammingLanguage>, IProgrammingLanguageAsyncDataService
    {
        public ProgrammingLanguagesAsyncDataService() : base(new UnitOfWork(new OrganizerContext()))
        {
        }
    }
}
