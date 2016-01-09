using ritchell.library.model.LibraryTransactions;
using ritchell.library.model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.Services
{
    public class LibrarianService
    {
        private readonly BookCopyService bookCopyService;
        private readonly BookService bookService;
        private readonly HolidayService holidayService;
        private readonly SectionService sectionService;

        public LibrarianService()
        {
            holidayService = new HolidayService();
            bookCopyService = new BookCopyService();
            sectionService = new SectionService();
            bookService = new BookService();

        }

        public void BorrowBook(LibraryUser libraryUser, string bookTag)
        {
            var bookCopy = bookCopyService.FindByShortRange(bookTag);

            if (bookCopy == null)
                throw new InvalidOperationException("Unknown Book.\nInform librarian to add book to database.");

            if (bookCopy.IsBorrowed == true)
                throw new InvalidOperationException("Book is already borrowed.\nKindly return it first before borrowing.");

            BookTransactionInfo bookTransaction = new BookTransactionInfo();

            bookTransaction.BookCopyId = bookCopy.Id;
            bookTransaction.LibraryUserId = libraryUser.Id;
            bookTransaction.IsTransactionDone = false;
            bookTransaction.BorrowDate = DateTime.Now;

            var bookInfo = bookCopyService.GetBookInfo(bookCopy);

            bookTransaction.ExpectedReturnDate = sectionService.GetBookReturnDateFromNow(bookInfo);

            bookCopy.IsBorrowed = true;

            using (var uow = new LibUnitOfWork())
            {
                //save transaction info to db
                uow.BookTransactionInfoRepository.Add(bookTransaction);
                uow.BookCopyRepository.Update(bookCopy);
                uow.SaveChanges();
            }
        }

        public void ReturnBook(string bookTag, DateTime returnDateTime, double amountPaid = 0)
        {
            var bookCopy = bookCopyService.FindByShortRange(bookTag);
            if (bookCopy.IsBorrowed == false)
                throw new InvalidOperationException("You are trying to return a book that is not 'borrowed'");

            bookCopy.IsBorrowed = false;

            using (var uow = new LibUnitOfWork())
            {
                var lastTrans = uow.BookTransactionInfoRepository.LastBookTransaction(bookCopy.Id);
                if (lastTrans == null)
                    throw new InvalidOperationException("Message for developer:\nBook is borrowed but no transaction record found. Why?");

                lastTrans.IsTransactionDone = true;
                lastTrans.ReturnDate = returnDateTime;

                if (lastTrans.ExpectedReturnDate.Date < returnDateTime.Date)
                {

                }
            }
        }
    }
}
