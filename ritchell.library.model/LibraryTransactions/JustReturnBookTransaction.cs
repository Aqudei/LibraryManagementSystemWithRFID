using ritchell.library.model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class JustReturnBookTransaction : LibraryTransactionBase
    {
        private TransactionInfo _LastTransaction;
        private PaymentService _PaymentService;

        public JustReturnBookTransaction(BookCopy bookCopy, TransactionInfo transInfo)
            : base(bookCopy)
        {
            _LastTransaction = transInfo;
            _PaymentService = new PaymentService();
        }

        public override string TransactionType
        {
            get
            {
                return "Return Only";
            }

            set
            { }
        }

        public override void Execute()
        {
            using (var uow = new LibUnitOfWork())
            {
                BookCopy.IsBorrowed = false;
                _LastTransaction.ReturnDate = DateTime.Now.Date;
                _LastTransaction.AmountToPay = _PaymentService.ComputeNecessaryFee(BookCopy, _LastTransaction);
                uow.BookTransactionInfoRepository.Update(_LastTransaction);
                uow.BookCopyRepository.Update(BookCopy);
                uow.SaveChanges();
            }
        }
    }
}
