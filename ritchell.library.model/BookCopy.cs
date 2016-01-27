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
            string bookTag, string longTag)
        {
            BookCopy bookCopy = new BookCopy();
            bookCopy.BookInfoId = bookInfo.Id;
            bookCopy.BookTagShort = bookTag;
            bookCopy.BookTagLong = longTag;
            return bookCopy;
        }

        public BookCopy()
        {
            Id = Guid.NewGuid();
        }

        public string BookTagShort { get; set; }
        public string BookTagLong { get; set; }
        public Guid BookInfoId { get; set; }

        public int AcquisitionNumber { get; set; }
        public bool IsBorrowed { get; set; }
    }
}
