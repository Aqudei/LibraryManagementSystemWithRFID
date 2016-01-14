using ritchell.library.model.Repositories;
using ritchell.library.model.Services;
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
        private double _RequiredFee;

        public ReturnBookTransaction(BookTransactionInfo lastBookTrans)
            : base(lastBookTrans.LibraryUserId, lastBookTrans.BookCopyId)
        {
            _LastBookTransaction = lastBookTrans;

            
            //(_LastBookTransaction.ExpectedReturnDate - DateTime.Now).Days
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
                return _RequiredFee;
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