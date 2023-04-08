using Organizer.Core.Models.Entities;
using Organizer.Data.UnitOfWork;
using Organizer.Infrastructure.Data;

namespace Organizer.Service.Data
{
    public class ProgrammingLanguagesAsyncDataService : BaseAsyncDataService<ProgrammingLanguage>
    {
        public ProgrammingLanguagesAsyncDataService() : base(new UnitOfWork(new OrganizerContext()))
        {
        }
    }
}
