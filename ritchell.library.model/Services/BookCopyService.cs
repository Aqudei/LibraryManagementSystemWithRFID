using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.model.Repositories;

namespace ritchell.library.model.Services
{
    public class BookCopyService
    {
        public void AddOrUpdateBookCopy(BookCopy bookCopy)
        {
            using (var uow = new LibUnitOfWork())
            {
                var existingBookCopy = uow.BookCopyRepository.Where(bc => bc.Id == bookCopy.Id).FirstOrDefault();
                if (existingBookCopy != null)
                {
                    var existing = uow.BookCopyRepository.Where(bc => bc.BookTagLong == bookCopy.BookTagLong && bc.Id != bookCopy.Id).FirstOrDefault();
                    if (existing != null)
                        throw new InvalidOperationException("That long-ranged RFID tag is already used");

                    existing = uow.BookCopyRepository.Where(bc => bc.BookTagShort == bookCopy.BookTagShort && bc.Id != bookCopy.Id).FirstOrDefault();
                    if (existing != null)
                        throw new InvalidOperationException("That short-ranged RFID tag is already used");

                    existing = uow.BookCopyRepository.Where(bc => bc.AcquisitionNumber == bookCopy.AcquisitionNumber && bc.Id != bookCopy.Id).FirstOrDefault();
                    if (existing != null)
                        throw new InvalidOperationException("Duplicate in Acquisition Number!");

                    bookCopy.IsBorrowed = false;

                    var lastTrans = uow.BookTransactionInfoRepository.GetLastBookTransaction(bookCopy.Id);
                    if (lastTrans != null)
                    {
                        lastTrans.ReturnDate = DateTime.Now;
                        lastTrans.AmountToPay = 0;
                        uow.BookTransactionInfoRepository.Update(lastTrans);
                    }

                    uow.BookCopyRepository.Update(bookCopy);
                }
                else
                {
                    var existing = uow.BookCopyRepository.FindByLongRangeRFId(bookCopy.BookTagLong);
                    if (existing != null)
                        throw new InvalidOperationException("That long-ranged RFID tag is already used");

                    existing = uow.BookCopyRepository.FindByShortRangeRFId(bookCopy.BookTagShort);
                    if (existing != null)
                        throw new InvalidOperationException("That short-ranged RFID tag is already used");

                    if (uow.BookCopyRepository.Where(bc => bc.AcquisitionNumber == bookCopy.AcquisitionNumber).Any())
                        throw new InvalidOperationException("Duplicate in Acquisition Number!");

                    uow.BookCopyRepository.Add(bookCopy);
                }
                uow.SaveChanges();
            }
        }

        public BookCopy FindById(object Id)
        {
            using (var bookCopyRepo = new BookCopyRepository())
            {
                return bookCopyRepo.FindById(Id);
            }
        }

        public IEnumerable<BookCopy> BookCopiesOf(Guid BookId)
        {
            using (var uow = new LibUnitOfWork())
            {
                return uow.BookCopyRepository.Where(bc => bc.BookInfoId.Equals(BookId)).ToList();
            }
        }

        public BookCopy FindByShortRange(string tag)
        {
            using (var uow = new LibUnitOfWork())
            {
                return uow.BookCopyRepository.Where(bc => bc.BookTagShort == tag).Single();
            }
        }

        public BookCopy FindByLongRange(string tag)
        {
            using (var uow = new LibUnitOfWork())
            {
                return uow.BookCopyRepository.Where(bc => bc.BookTagLong == tag).Single();
            }
        }

        public void RemoveBookCopy(Guid BookCopyId)
        {
            using (var uow = new LibUnitOfWork())
            {
                var bookCopy = uow.BookCopyRepository.FindById(BookCopyId);
                if (bookCopy != null)
                {
                    uow.BookCopyRepository.Remove(bookCopy);
                    uow.SaveChanges();
                }
            }
        }

        public BookInfo GetBookInfo(BookCopy bookCopy)
        {
            using (var bookInfoRepo = new BookInfoRepository())
            {
                return bookInfoRepo.Where(b => b.Id.Equals(bookCopy.BookInfoId)).Single();
            }
        }

        public BookInfo GetBookInfo(Guid bookCopyId)
        {
            using (var bookInfoRepo = new BookInfoRepository())
            using (var bookCopyRepo = new BookCopyRepository())
            {
                var bookCopy = bookCopyRepo.FindById(bookCopyId);
                return bookInfoRepo.Where(b => b.Id.Equals(bookCopy.BookInfoId)).Single();
            }
        }

        public IEnumerable<BookCopy> GetBorrowedBooks()
        {
            using (var bookCopyRepo = new BookCopyRepository())
            {
                return bookCopyRepo.Where(b => b.IsBorrowed == true);
            }
        }
    }
}
