using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;

namespace ritchell.library.model.Interfaces
{
    public interface ISectionRepository : IRepository<Section>,IDisposable
    {
        int GetNumberOfBooks(Guid SectionId);
        IEnumerable<BookInfo> GetBooks(Guid SectionId);
    }
}
