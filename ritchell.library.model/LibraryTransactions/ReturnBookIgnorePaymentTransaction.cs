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
        private TransactionInfo _LastTransaction;
        
        public ReturnBookIgnorePaymentTransaction(BookCopy bookCopy, TransactionInfo transInfo)
            : base(bookCopy)
        {
            _LastTransaction = transInfo;
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
                _LastTransaction.IsPaid = true;
                _LastTransaction.ReturnDate = DateTime.Now.Date;
                uow.BookTransactionInfoRepository.Update(_LastTransaction);
                uow.BookCopyRepository.Update(BookCopy);
                uow.SaveChanges();
            }
        }
    }
}
