using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using ritchell.library.model.Interfaces;

namespace ritchell.library.model.Repositories
{
    public class SectionRepository : RepositoryBase<Section>, ISectionRepository
    {
        public SectionRepository(DbContext context)
            : base(context)
        { }

        public int GetNumberOfBooks(Guid SectionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookInfo> GetBooks(Guid SectionId)
        {
            return _Context.Set<Section>()
                .Include(s => s.BookInfos)
                .Where(s => s.Id == SectionId)
                .Single().BookInfos;
        }
    }
}
