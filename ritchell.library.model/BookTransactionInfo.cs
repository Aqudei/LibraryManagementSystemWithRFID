using ritchell.library.infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model
{
    public class BookTransactionInfo : EntityBase<Guid>
    {
        public Guid BookCopyId { get; internal set; }
        public Guid LibraryUserId { get; internal set; }
        public bool IsTransactionDone { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public Double AmountPaid { get; set; }

        public BookTransactionInfo()
        {
            Id = Guid.NewGuid();
        }

    }
}
