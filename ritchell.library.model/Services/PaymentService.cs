using ritchell.library.model.Repositories;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class PaymentService
    {
        private BookService _BookInfoService;
        private SectionService _SectionService;
        private BookCopyService _BookCopyService;
        private LibraryUserService _LibraryUserService;
        private BookTransactionInfoRepository _BookTransactionInfoRepository;
        private PaymentService _PaymentService;

        public PaymentService()
        {
            _SectionService = new SectionService();
            _BookInfoService = new BookService();
            _BookCopyService = new BookCopyService();
            _LibraryUserService = new LibraryUserService();
            _BookTransactionInfoRepository = new BookTransactionInfoRepository();
        }

        public double ComputeNecessaryFee(BookCopy bookCopy, TransactionInfo bookTransInfo)
        {
            var bookInfo = _BookCopyService.GetBookInfo(bookCopy);

            if (bookInfo == null)
                throw new InvalidOperationException("Book copy has no known book information.");

            var section = _SectionService.GetBookSection(bookInfo);

            if (section == null)
                throw new InvalidOperationException("The book does not belong to a section.");

            if (DateTime.Now.Date <= bookTransInfo.ExpectedReturnDate.Date)
                return 0;

            return (DateTime.Now.Date - bookTransInfo.ExpectedReturnDate.Date).Days * section.LateReturningFee;
        }

        public void CompletePayment(Payable payable)
        {
            var transInfo = payable.TransactionInfo;

            if (transInfo.AmountToPay <= 0 || transInfo.IsPaid == true)
                throw new ArgumentException("No need to pay for this transaction.");

            transInfo.IsPaid = true;

            var bookCopy = _BookCopyService.FindById(transInfo.BookCopyId);

            bookCopy.IsBorrowed = false;

            using (var uow = new library.model.Repositories.LibUnitOfWork())
            {
                uow.BookCopyRepository.Update(bookCopy);
                uow.BookTransactionInfoRepository.Update(transInfo);
                uow.SaveChanges();
            }

        }

        public IEnumerable<Payable> GetReturnedBooksPayables(LibraryUser user)
        {
            return GetReturnedBooksPayables().Where(p => p.LibraryUserId.Equals(user.Id)).ToList();
        }

        public IEnumerable<Payable> GetReturnedBooksPayables()
        {
            ICollection<Payable> Payables = new List<Payable>();

            using (var transRepo = new BookTransactionInfoRepository())
            using (var userRepo = new LibraryUserRepository())
            {
                var trans = transRepo.GetReturnedBooksPayableTransactions();
                foreach (var tran in trans)
                {
                    Payable p = new Payable
                    {
                        TransactionInfo = tran,
                        LibraryUserId = tran.LibraryUserId,
                        AmountToPay = tran.AmountToPay,
                        BookInvolved = _BookCopyService.GetBookInfo(tran.BookCopyId).BookTitle,
                        UserInvolved = userRepo.FindById(tran.LibraryUserId).Fullname
                    };
                    Payables.Add(p);
                }
                return Payables;
            }
        }

        public IEnumerable<ReturnBookDTO> GetBorrowedBooks()
        {
            List<ReturnBookDTO> libTrans = new List<ReturnBookDTO>();
            var books = _BookCopyService.GetBorrowedBooks();
            foreach (var book in books)
            {

                var bookInfo = _BookInfoService.BookInfoOf(book);
                var lastTrans = _BookTransactionInfoRepository.GetLastBookTransaction(book.Id);
                if (lastTrans != null)
                {
                    var rqFee = ComputeNecessaryFee(book, lastTrans);
                    var user = _LibraryUserService.FindById(lastTrans.LibraryUserId);
                    if (user.LibraryUserType == LibraryUser.UserType.Student)
                    {
                        var trans = new ReturnBookTransaction(book, lastTrans);
                        trans.CompletePayment();
                        libTrans.Add(new ReturnBookDTO
                        {
                            TransactionInfo = trans,
                            LibraryUser = user,
                            BookInfo = bookInfo,
                            RequiredFee = rqFee
                        });
                    }
                    else if (user.LibraryUserType == LibraryUser.UserType.Teacher)
                    {
                        var trans = new ReturnBookIgnorePaymentTransaction(book, lastTrans);
                        libTrans.Add(new ReturnBookDTO
                        {
                            TransactionInfo = trans,
                            LibraryUser = user,
                            BookInfo = bookInfo,
                            RequiredFee = rqFee
                        });
                    }
                }
                else
                    Debug.WriteLine("Possible error. A book was borrowed but no transaction info was found.");
            }
            return libTrans;
        }
    }
}
