using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure
{
    public interface IRepository<T> where T : class
    {
        T FindById(object Id);
        void Add(T item);
        void Remove(T item);
        void Update(T item);
        IEnumerable<T> GetAll();
        IEnumerable<T> Where(Expression<Func<T, bool>> filter);
    }
}
