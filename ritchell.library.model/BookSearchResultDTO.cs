using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model
{
    public class BookSearchResultDTO
    {
        public int NumberOfCopies { get; set; }
        public int AvailableCopies { get; set; }
        public BookInfo BookInfo { get; set; }
        public Section Section { get; set; }
    }
}
