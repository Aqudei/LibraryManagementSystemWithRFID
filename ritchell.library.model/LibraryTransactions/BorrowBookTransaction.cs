using ritchell.library.model.Repositories;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class BorrowBookTransaction : ILibraryTransaction
    {
        private readonly BookCopyService bookCopyService;
        private readonly HolidayService holidayService;

        public Guid LibraryUserId { get; set; }
        public string BookTag { get; set; }

        public DateTime TransactionDate { get; set; }

        public BorrowBookTransaction()
        {
            bookCopyService = new BookCopyService();
            holidayService = new HolidayService();
            TransactionDate = DateTime.Now;
        }


        public void Execute()
        {
            using (var uow = new LibUnitOfWork())
            {
                var bookCopy = uow.BookCopyRepository.FindByShortRangeRFId(BookTag);
                if (bookCopy.IsBorrowed)
                    throw new InvalidOperationException("Book already borrowed");

                bookCopy.IsBorrowed = true;

                var bookInfo = uow.BookInfoRepository.BookInfoOf(bookCopy);
                var section = uow.SectionRepository.Where(s => s.Id.Equals(bookInfo.SectionId)).Single();

                BookTransactionInfo bookTransInfo = new BookTransactionInfo
                {
                    BookCopyId = bookCopy.Id,
                    BorrowDate = TransactionDate,
                    IsTransactionDone = false,
                    LibraryUserId = LibraryUserId,
                    ExpectedReturnDate = holidayService.GetNonHolidayDateAfter(TransactionDate.AddDays(section.MaxDaysAllowedForBorrowing))
                };

                uow.BookCopyRepository.Update(bookCopy);
                uow.BookTransactionInfoRepository.Add(bookTransInfo);
                uow.SaveChanges();
            }
        }
    }
}
