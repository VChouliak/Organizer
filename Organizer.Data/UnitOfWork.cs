using Microsoft.EntityFrameworkCore;
using Organizer.Core.Interfaces.Data;
using Organizer.Core.Models;
using Organizer.Data.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IBaseAsyncRepository<TModel> Repository<TModel>() where TModel : BaseModel
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }
            var type = typeof(TModel).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseAsyncRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TModel)), _context);
                _repositories.Add (type, repositoryInstance);
            }

            return _repositories[type] as IBaseAsyncRepository<TModel>;
        }
    }
}
