using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
