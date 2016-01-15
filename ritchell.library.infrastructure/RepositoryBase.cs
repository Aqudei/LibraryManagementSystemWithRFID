using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure
{
    public class RepositoryBase<Ttype> : IRepository<Ttype> where Ttype : EntityBase<Guid>
    {
        protected DbContext _Context;

        public RepositoryBase(DbContext context)
        {
            _Context = context;
        }

        public void Add(Ttype item)
        {
            _Context.Set<Ttype>().Add(item);
        }

        public void Remove(Ttype item)
        {
            var retrievedItem = _Context.Set<Ttype>().Find(item.Id);
            if (retrievedItem != null)
                _Context.Set<Ttype>().Remove(retrievedItem);
        }

        public void Update(Ttype item)
        {
            var old = _Context.Set<Ttype>().Find(item.Id);
            _Context.Entry(old).CurrentValues.SetValues(item);
        }

        public IEnumerable<Ttype> GetAll()
        {
            return _Context.Set<Ttype>().ToList();
        }

        public IEnumerable<Ttype> Where(System.Linq.Expressions.Expression<Func<Ttype, bool>> filter)
        {
            return _Context.Set<Ttype>().Where(filter).ToList();
        }

        public Ttype FindById(object Id)
        {
            return _Context.Set<Ttype>().Find(Id);
        }
    }
}
