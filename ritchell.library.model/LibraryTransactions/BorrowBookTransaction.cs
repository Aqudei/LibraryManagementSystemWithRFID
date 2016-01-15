using ritchell.library.model.Repositories;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class BorrowBookTransaction : LibraryTransactionBase
    {
        private readonly BookCopyService bookCopyService;
        private readonly HolidayService holidayService;        

        public override string TransactionType
        {
            get
            {
                return "Borrow";
            }
        }

        public BorrowBookTransaction(Guid libUserId, Guid booCopyId)
            : base(libUserId, booCopyId)
        {
            bookCopyService = new BookCopyService();
            holidayService = new HolidayService();
        }

        public override void Execute()
        {
            using (var uow = new LibUnitOfWork())
            {
                if (BookCopy.IsBorrowed)
                    throw new InvalidOperationException("Book already borrowed");

                BookCopy.IsBorrowed = true;

                var bookInfo = uow.BookInfoRepository.BookInfoOf(BookCopy);
                var section = uow.SectionRepository.Where(s => s.Id.Equals(bookInfo.SectionId)).Single();

                BookTransactionInfo bookTransInfo = new BookTransactionInfo
                {
                    BookCopyId = BookCopy.Id,
                    BorrowDate = TransactionDate,
                    IsTransactionDone = false,
                    LibraryUserId = LibraryUserId,
                    ExpectedReturnDate = holidayService.GetNonHolidayDateAfter(TransactionDate.AddDays(section.MaxDaysAllowedForBorrowing))
                };

                uow.BookCopyRepository.Update(BookCopy);
                uow.BookTransactionInfoRepository.Add(bookTransInfo);
                uow.SaveChanges();
            }
        }
    }
}
