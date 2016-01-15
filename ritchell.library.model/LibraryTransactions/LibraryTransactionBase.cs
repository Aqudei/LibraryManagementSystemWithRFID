using ritchell.library.infrastructure;
using ritchell.library.model.Repositories;
using System;

namespace ritchell.library.model.LibraryTransactions
{
    public abstract class LibraryTransactionBase : INPCBase
    {
        public abstract string TransactionType { get; }
        public string BookTitle { get; set; }
        public string BookTag { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid LibraryUserId { get; set; }
        protected BookCopy BookCopy { get; set; }

        public abstract void Execute();

        public LibraryTransactionBase(Guid libUserId, Guid bookCopyId)
        {
            LibraryUserId = libUserId;

            using (var bookCopyRepo = new BookCopyRepository())
            {
                BookCopy = bookCopyRepo.FindById(bookCopyId);
                if (BookCopy == null)
                    throw new InvalidOperationException("Unknown book: " + BookTag);

                BookTag = BookCopy.BookTagShort;
                BookTitle = bookCopyRepo.BookInfoOf(BookCopy).BookTitle;
                TransactionDate = DateTime.Now;
            }
        }
    }
}
