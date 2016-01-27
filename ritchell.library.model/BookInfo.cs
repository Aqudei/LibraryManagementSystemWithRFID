using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;

namespace ritchell.library.model
{
    public class BookInfo : EntityBase<Guid>
    {
        public string BookTitle { get; set; }
        public string CallNumber { get; set; }
        public string Author { get; set; }
        public int Copyright { get; set; }
        public string ISBN { get; set; }
        public Guid SectionId { get; set; }
        public string Subject { get; set; }
        public virtual ICollection<BookCopy> BookCopies { get; set; }

        public BookInfo()
        {
            Id = Guid.NewGuid();
            BookCopies = new List<BookCopy>();
        }
    }
}
