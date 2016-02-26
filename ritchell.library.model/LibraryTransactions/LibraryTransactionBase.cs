using ritchell.library.infrastructure;
using ritchell.library.infrastructure.Logging;
using ritchell.library.model.Logging;
using ritchell.library.model.Repositories;
using ritchell.library.model.Services;
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
        public BookCopy BookCopy { get; set; }

        public IActionLogger ActionLogger
        {
            get
            {
                return actionLogger = actionLogger ?? new DBLogger();
            }
        }

        public LibraryUserService UserService
        {
            get
            {
                return _UserService;
            }

            set
            {
                _UserService = value;
            }
        }

        public abstract void Execute();

        public LibraryTransactionBase(BookCopy bookCopy)
        {
            BookCopy = bookCopy;

            using (var bookCopyRepo = new BookCopyRepository())
            {
                BookTag = BookCopy.BookTagShort;
                BookTitle = bookCopyRepo.BookInfoOf(BookCopy).BookTitle;
                TransactionDate = DateTime.Now;
            }

            UserService = new LibraryUserService();
        }

        private IActionLogger actionLogger;
        private LibraryUserService _UserService;
    }
}
