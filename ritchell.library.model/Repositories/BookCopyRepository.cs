using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using ritchell.library.model.Interfaces;

namespace ritchell.library.model.Repositories
{
    public class BookCopyRepository : RepositoryBase<BookCopy>, IBookCopyRepository
    {
        public BookCopyRepository(System.Data.Entity.DbContext context)
            : base(context)
        { }

        
    }
}
