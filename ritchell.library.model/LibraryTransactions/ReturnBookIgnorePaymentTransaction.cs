using ritchell.library.model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class ReturnBookIgnorePaymentTransaction : LibraryTransactionBase
    {
        private string _TransactionType;
        private BookTransactionInfo _LastBookTransaction;
        
        public ReturnBookIgnorePaymentTransaction(BookTransactionInfo bookTransInfo)
            : base(bookTransInfo.LibraryUserId, bookTransInfo.BookCopyId)
        {
            _LastBookTransaction = bookTransInfo;
        }

        public override string TransactionType
        {
            get
            {
                return _TransactionType = _TransactionType ?? "Return Book: ";
            }

            set
            {
                _TransactionType = value;
            }
        }

        public override void Execute()
        {
            using (var uow = new LibUnitOfWork())
            {
                BookCopy.IsBorrowed = false;
                _LastBookTransaction.IsTransactionDone = true;
                _LastBookTransaction.ReturnDate = DateTime.Now.Date;
                uow.BookTransactionInfoRepository.Update(_LastBookTransaction);
                uow.BookCopyRepository.Update(BookCopy);
                uow.SaveChanges();
            }
        }
    }
}
