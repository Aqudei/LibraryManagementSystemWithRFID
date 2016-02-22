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
        private TransactionInfo _LastTransaction;
        private PaymentService _PaymentService;

        public ReturnBookTransaction(BookCopy bookCopy, TransactionInfo transInfo)
            : base(bookCopy)
        {
            _PaymentService = new PaymentService();
            _LastTransaction = transInfo;
            _LastTransaction.AmountToPay = RequiredFee;
        }

        public double RequiredFee
        {
            get
            {
                return _PaymentService.ComputeNecessaryFee(BookCopy, _LastTransaction);
            }
        }

        public void CompletePayment()
        {
            _LastTransaction.IsPaid = true;
            _LastTransaction.DateOfPayment = DateTime.Now;
        }

        public override string TransactionType
        {
            get
            {
                return "Return Book";
            }
            set
            { }
        }

        public override void Execute()
        {
            using (var uow = new LibUnitOfWork())
            {
                BookCopy.IsBorrowed = false;
                _LastTransaction.ReturnDate = DateTime.Now;
                uow.BookTransactionInfoRepository.Update(_LastTransaction);
                uow.BookCopyRepository.Update(BookCopy);
                uow.SaveChanges();
            }
        }
    }
}