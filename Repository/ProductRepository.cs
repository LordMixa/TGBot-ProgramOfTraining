using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleg_training.DBEntities;

namespace Teleg_training.Repository
{
    internal class ProductRepository : IRepository<DBProduct>, IDisposable
    {
        public readonly DbSet<DBProduct> _productSet;
        private ProgramListContext _context;
        private bool disposed = false;
        public ProductRepository(ProgramListContext programListContext)
        {
            _context = programListContext;
            _productSet = programListContext.Products;
        }
        public void Create(DBProduct item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
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

        public DBProduct Get(int id)
        {
            return _productSet.Find(id);
        }

        public IEnumerable<DBProduct> GetAll()
        {
            return _productSet;
        }

        public void Update(int id, DBProduct item)
        {
            throw new NotImplementedException();
        }
    }
}
