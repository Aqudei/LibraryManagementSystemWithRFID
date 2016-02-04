using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
            AcquisitionNumber = 1;
        }

        [Index(IsUnique = true)]
        [StringLength(256)]
        public string BookTagShort { get; set; }

        [Index(IsUnique = true)]
        [StringLength(256)]
        public string BookTagLong { get; set; }
        public Guid BookInfoId { get; set; }

        [Index(IsUnique = true)]
        public int AcquisitionNumber { get; set; }
        public bool IsBorrowed { get; set; }
    }
}
