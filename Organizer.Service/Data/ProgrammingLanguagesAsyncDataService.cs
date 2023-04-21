using Organizer.Core.Interfaces.Data;
using Organizer.Core.Interfaces.Services;
using Organizer.Core.Models.Entities;
using Organizer.Data.UnitOfWork;
using Organizer.Infrastructure.Data;

namespace Organizer.Service.Data
{
    public class ProgrammingLanguagesAsyncDataService : BaseAsyncDataService<ProgrammingLanguage>, IProgrammingLanguageAsyncDataService
    {
        public ProgrammingLanguagesAsyncDataService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
