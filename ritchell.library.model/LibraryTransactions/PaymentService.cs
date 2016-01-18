using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class PaymentService
    {
        private BookService _BookInfoService;
        private SectionService _SectionService;

        public PaymentService()
        {
            _SectionService = new SectionService();
            _BookInfoService = new BookService();
        }

        public double ComputeNecessaryFee(BookCopy bookCopy, TransactionInfo bookTransInfo)
        {
            var bookCopyService = new BookCopyService();

            var bookInfo = bookCopyService.GetBookInfo(bookCopy);

            if (bookInfo == null)
                throw new InvalidOperationException("Book copy has no known book information.");

            var section = _SectionService.GetBookSection(bookInfo);

            if (section == null)
                throw new InvalidOperationException("The book does not belong to a section.");

            if (DateTime.Now.Date <= bookTransInfo.ExpectedReturnDate.Date)
                return 0;

            return (DateTime.Now.Date - bookTransInfo.ExpectedReturnDate.Date).Days * section.LateReturningFee;
        }
    }
}
