using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;

namespace ritchell.library.model.Interfaces
{
    public interface IBookInfoRepository : IRepository<BookInfo>, IDisposable
    {
        BookInfo BookInfoOf(BookCopy bookCopy);
        int GetNumberOfCopies(BookInfo bookInfo);
        int GetNumberOfAvailableCopies(BookInfo bookInfo);
        IEnumerable<BookInfo> SearchForBooks(string keyword);
    }
}
