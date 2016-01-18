using ritchell.library.infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class TransactionInfo : EntityBase<Guid>, IComparable<TransactionInfo>
    {
        public Guid BookCopyId { get; set; }
        public Guid LibraryUserId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public bool IsPaid { get; set; }
        public double AmountToPay { get; set; }

        public TransactionInfo()
        {
            Id = Guid.NewGuid();
        }

        public int CompareTo(TransactionInfo other)
        {
            return this.BorrowDate.Value.CompareTo(other.BorrowDate);
        }
    }
}
