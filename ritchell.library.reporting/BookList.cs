using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.reporting
{
    public class BookList
    {
        public string ISBN13 { get; set; }
        public string ISBN10 { get; set; }
        public string Author { get; set; }
        public string BookTitle { get; set; }
        public string BookSection { get; set; }
        public int NumberOfCopies { get; set; }
        public int Available { get; set; }
        public int Borrowed { get; set; }
    }
}
