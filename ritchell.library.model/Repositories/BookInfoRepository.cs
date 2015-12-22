using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using ritchell.library.model.Interfaces;

namespace ritchell.library.model.Repositories
{
    public class BookInfoRepository : RepositoryBase<BookInfo>, IBookInfoRepository
    {
        public BookInfoRepository(System.Data.Entity.DbContext _Context)
            : base(_Context)
        {
            // TODO: Complete member initialization
            this._Context = _Context;
        }
    }
}
