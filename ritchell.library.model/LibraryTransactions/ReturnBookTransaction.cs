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
        private PaymentService _PaymentService;

        public ReturnBookTransaction(BookTransactionInfo lastBookTrans)
            : base(lastBookTrans.LibraryUserId, lastBookTrans.BookCopyId)
        {
            _LastBookTransaction = lastBookTrans;
            _PaymentService = new PaymentService();
        }

        public double RequiredFee
        {
            get
            {
                return _PaymentService.ComputeNecessaryFee(BookCopy) - _LastBookTransaction.AmountPaid;
            }
        }

        public void CompletePayment()
        {
            _LastBookTransaction.AmountPaid = _PaymentService.ComputeNecessaryFee(BookCopy);
            FirePropertyChanged("RequiredFee");
        }

        public override string TransactionType
        {
            get
            {
                return "Return";
            }
        }

        public override void Execute()
        {
            using (var uow = new LibUnitOfWork())
            {
                BookCopy.IsBorrowed = false;
                _LastBookTransaction.IsTransactionDone = true;
                _LastBookTransaction.ReturnDate = DateTime.Now;
                _LastBookTransaction.AmountPaid = _PaymentService.ComputeNecessaryFee(BookCopy);
                uow.BookTransactionInfoRepository.Update(_LastBookTransaction);
                uow.BookCopyRepository.Update(BookCopy);
                uow.SaveChanges();
            }
        }
    }
}