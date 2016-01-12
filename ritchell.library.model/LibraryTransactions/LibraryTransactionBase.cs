using ritchell.library.model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public abstract class LibraryTransactionBase
    {
        public abstract string TransactionType { get; }
        public string BookTitle { get; set; }
        public string BookTag { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid LibraryUserId { get; set; }
        protected BookCopy BookCopy { get; set; }

        public abstract void Execute();

        public LibraryTransactionBase(Guid libUserId, string bookTag)
        {
            LibraryUserId = libUserId;
            BookTag = BookTag;

            using (var bookCopyRepo = new BookCopyRepository())
            {
                BookCopy = bookCopyRepo.FindByShortRangeRFId(BookTag);

                if (BookCopy == null)
                    throw new InvalidOperationException("Unknown book: " + BookTag);

                BookTitle = bookCopyRepo.BookInfoOf(BookCopy).BookTitle;
                TransactionDate = DateTime.Now;
            }
        }
    }
}
