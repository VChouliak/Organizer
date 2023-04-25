using Microsoft.EntityFrameworkCore;
using Organizer.Core.Interfaces.Data;
using Organizer.Core.Interfaces.Services;
using Organizer.Core.Models.Entities;
using Organizer.Data.UnitOfWork;
using Organizer.Infrastructure.Data;

namespace Organizer.Service.Data
{
    public class ProgrammingLanguagesAsyncDataService : BaseAsyncDataService<ProgrammingLanguage>, IProgrammingLanguageAsyncDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProgrammingLanguagesAsyncDataService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsReferencedByFriendAsync(int programmingLanguageId)
        {
            var dbContext = ((UnitOfWork)_unitOfWork).GetDbContext();
            return await dbContext.Set<Friend>().AsNoTracking().AnyAsync(f=>f.ProgrammingLanguageId == programmingLanguageId);
        }
    }
}
