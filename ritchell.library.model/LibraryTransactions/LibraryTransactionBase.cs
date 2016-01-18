using ritchell.library.infrastructure;
using ritchell.library.model.Repositories;
using System;

namespace ritchell.library.model.LibraryTransactions
{
    public abstract class LibraryTransactionBase : INPCBase
    {
        public enum TransactionStatus
        {
            Pending, Success, Failed
        }


        public TransactionStatus LibraryTransactionStatus { get; set; }
        public abstract string TransactionType { get; set; }
        public string BookTitle { get; set; }
        public string BookTag { get; set; }
        public DateTime TransactionDate { get; set; }
        public LibraryUser LibraryUser { get; set; }
        protected BookCopy BookCopy { get; set; }

        public abstract void Execute();

        public LibraryTransactionBase(LibraryUser libUser, BookCopy bookCopy)
        {
            LibraryUser = libUser;

            using (var bookCopyRepo = new BookCopyRepository())
            {
                BookTag = BookCopy.BookTagShort;
                BookTitle = bookCopyRepo.BookInfoOf(BookCopy).BookTitle;
                TransactionDate = DateTime.Now;
            }
        }
    }
}
