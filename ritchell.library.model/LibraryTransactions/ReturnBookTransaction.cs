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
        private double payment;

        public ReturnBookTransaction(Guid libUserId, string bookTag)
            : base(libUserId, bookTag)
        {
            payment = 0;
        }

        public ReturnBookTransaction WithPayment(double payment)
        {
            this.payment += payment;
            return this;
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
                var lastTransaction = uow.BookTransactionInfoRepository.LastBookTransaction(BookCopy.Id);
                if (lastTransaction == null)
                    throw new InvalidOperationException("The book has no known transactions.");

                if (BookCopy.IsBorrowed)
                {
                    BookCopy.IsBorrowed = false;
                    lastTransaction.IsTransactionDone = true;
                    lastTransaction.ReturnDate = DateTime.Now;
                    lastTransaction.AmountPaid = payment;
                }
                else
                    throw new InvalidOperationException("The book is not borrowed");

                uow.SaveChanges();
            }
        }
    }
}