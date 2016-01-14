using ritchell.library.model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class ReturnBookTransaction : LibraryTransactionBase
    {
        private BookTransactionInfo _LastBookTransaction;

        public ReturnBookTransaction(BookTransactionInfo lastBookTrans)
            : base(lastBookTrans.LibraryUserId, lastBookTrans.BookCopyId)
        {
            _LastBookTransaction = lastBookTrans;
        }

        public ReturnBookTransaction PayNecessaryFee()
        {
            _LastBookTransaction.AmountPaid = RequiredFee;
            return this;
        }

        public override string TransactionType
        {
            get
            {
                return "Return";
            }
        }

        public double RequiredFee
        {
            get
            {
                return 0;
                //(_LastBookTransaction.ExpectedReturnDate - DateTime.Now).Days
            }
        }


        public override void Execute()
        {
            using (var uow = new LibUnitOfWork())
            {
                BookCopy.IsBorrowed = false;
                _LastBookTransaction.IsTransactionDone = true;
                _LastBookTransaction.ReturnDate = DateTime.Now;

                uow.BookTransactionInfoRepository.Update(_LastBookTransaction);
                uow.BookCopyRepository.Update(BookCopy);

                uow.SaveChanges();
            }
        }
    }
}