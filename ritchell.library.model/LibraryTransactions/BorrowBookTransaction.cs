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
        private readonly BookCopyService _BookCopyService;
        private readonly HolidayService _HolidayService;
        private SectionService _SectionService;
        private BookService _BookInfoService;

        private string _TransactionType = "Borrow";
        private BookTransactionInfo bookTransInfo;

        public override string TransactionType
        {
            get
            {
                return _TransactionType;
            }

            set
            {
                _TransactionType = value;
                FirePropertyChanged("TransactionType");
            }
        }

        public BorrowBookTransaction(Guid libUserId, Guid booCopyId)
            : base(libUserId, booCopyId)
        {
            _BookCopyService = new BookCopyService();
            _HolidayService = new HolidayService();
            _BookInfoService = new BookService();
            _SectionService = new SectionService();

            var bookInfo = _BookInfoService.BookInfoOf(BookCopy);
            var section = _SectionService.GetBookSection(bookInfo);

            if (section == null)
                throw new InvalidOperationException("The book does not belong to a section.");

            if (section.MaxDaysAllowedForBorrowing == 0)
                TransactionType = "Not Allowed For Borrowing.";

            bookTransInfo = new BookTransactionInfo
            {
                BookCopyId = BookCopy.Id,
                LibraryUserId = LibraryUserId,
                BorrowDate = TransactionDate,
                IsTransactionDone = false,
                ExpectedReturnDate = _HolidayService.GetNonHolidayDateAfter(TransactionDate.AddDays(section.MaxDaysAllowedForBorrowing))
            };
        }

        public override void Execute()
        {
            using (var uow = new LibUnitOfWork())
            {
                BookCopy.IsBorrowed = true;

                var bookInfo = uow.BookInfoRepository.BookInfoOf(BookCopy);
                var section = uow.SectionRepository.Where(s => s.Id.Equals(bookInfo.SectionId)).Single();

                if (section.MaxDaysAllowedForBorrowing == 0)
                    return;

                uow.BookCopyRepository.Update(BookCopy);
                uow.BookTransactionInfoRepository.Add(bookTransInfo);
                uow.SaveChanges();
            }
        }
    }
}
