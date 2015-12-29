using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;

namespace ritchell.library.model
{
    public class BookCopy : EntityBase<Guid>
    {
        public static BookCopy MakeCopy(model.BookInfo bookInfo,
            string bookTag)
        {
            BookCopy bookCopy = new BookCopy();
            bookCopy.BookInfoId = bookInfo.Id;
            bookCopy.BookTag = bookTag;

            return bookCopy;
        }

        public BookCopy()
        {
            Id = Guid.NewGuid();
        }

        public string BookTag { get; set; }
        public string BookTagLong { get; set; }
        public Guid BookInfoId { get; set; }
    }
}
