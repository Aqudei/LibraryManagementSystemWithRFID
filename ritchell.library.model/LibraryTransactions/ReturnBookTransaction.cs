using ritchell.library.model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class ReturnBookTransaction : ILibraryTransaction
    {
        private DateTime _TransactionDate;
        private string _BookTag;
        private double payment = 0;

        public static ReturnBookTransaction Create(string bookTag)
        {
            return new ReturnBookTransaction
            {
                BookTag = bookTag,

            };
        }

        public ReturnBookTransaction WithPayment(double payment)
        {
            this.payment += payment;
            return this;
        }

        public string BookTag
        {
            get { return _BookTag; }
            set { _BookTag = value; }
        }

        public DateTime TransactionDate
        {
            get { return _TransactionDate; }
            set { _TransactionDate = value; }
        }

        public void Execute()
        {
            using (var uow = new LibUnitOfWork())
            {
                var bookCopy = uow.BookCopyRepository.FindByShortRangeRFId(BookTag);
                if (bookCopy == null)
                    throw new InvalidOperationException("Unknown book");

                var lastTransaction = uow.BookTransactionInfoRepository.LastBookTransaction(bookCopy.Id);
                if (lastTransaction == null)
                    throw new InvalidOperationException("The book has no known transactions.");

                if (bookCopy.IsBorrowed)
                {
                    bookCopy.IsBorrowed = false;
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