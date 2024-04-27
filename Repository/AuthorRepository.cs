using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleg_training.DBEntities;

namespace Teleg_training.Repository
{
    public class AuthorRepository : IRepository<DBAuthor>,IDisposable
    {
        public readonly DbSet<DBAuthor> _authorSet;
        private ProgramListContext _context;
        private bool disposed = false;
        public AuthorRepository(ProgramListContext programListContext)
        {
            _context = programListContext;
            _authorSet = programListContext.Authors;
        }
        DBAuthor IRepository<DBAuthor>.Get(int id)
        {
            return _authorSet.Find(id);
        }

        void IRepository<DBAuthor>.Create(DBAuthor item)
        {
            throw new NotImplementedException();
        }

        void IRepository<DBAuthor>.Update(int id, DBAuthor item)
        {
            throw new NotImplementedException();
        }

        void IRepository<DBAuthor>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DBAuthor> GetAll()
        {
            return _authorSet;
        }
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
