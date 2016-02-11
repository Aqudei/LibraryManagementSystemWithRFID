using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model
{
    public class ReturnBookDTO
    {
        public LibraryTransactions.LibraryTransactionBase TransactionInfo { get; set; }
        public LibraryUser LibraryUser { get; set; }
        public BookInfo BookInfo { get; set; }
        public Double RequiredFee { get; set; }
        public BookCopy BookCopy { get; set; }
    }
}
