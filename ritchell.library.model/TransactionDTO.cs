using ritchell.library.model.LibraryTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model
{
    public class TransactionDTO
    {
        public TransactionInfo TransactionInfo { get; set; }
        public LibraryUser LibraryUser { get; set; }
        public BookInfo BookInfo { get; set; }
        public BookCopy BookCopy { get; set; }
    }
}
