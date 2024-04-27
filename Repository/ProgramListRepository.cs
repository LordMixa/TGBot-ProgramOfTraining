using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleg_training.DBEntities;

namespace Teleg_training.Repository
{
    internal class ProgramListRepository : IRepository<DBProgramList>, IDisposable
    {
        public readonly DbSet<DBProgramList> _programListSet;
        private ProgramListContext _context;
        private bool disposed = false;
        public ProgramListRepository(ProgramListContext context)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<ProgramListContext>();
            //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TGBotProgram;Trusted_Connection=True;");
            //var dbContextOptions = optionsBuilder.Options;
            _context = context;
            _programListSet = _context.ProgramLists;
        }
        public void Create(DBProgramList item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public DBProgramList Get(int id)
        {
            return _programListSet.Find(id);
        }
        public DBProgramList GetbyName(string Name)
        {
            return _programListSet.Where(p => p.Name == Name).FirstOrDefault();
        }
        public IEnumerable<DBProgramList> GetAll()
        {
            return _programListSet;
        }

        public void Update(int id, DBProgramList item)
        {
            _programListSet.Update(item);
            _context.SaveChanges();
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
