using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.model.Repositories;

namespace ritchell.library.model.Services
{
    public class BookService
    {
        public void AddOrUpdateBook(BookInfo bookInfo)
        {
            using (LibUnitOfWork uow = new LibUnitOfWork())
            {
                var book = uow.BookInfoRepository.FindById(bookInfo.Id);

                if (book == null)
                    uow.BookInfoRepository.Add(bookInfo);
                else
                    uow.BookInfoRepository.Update(bookInfo);

                uow.SaveChanges();
            }
        }

        public IEnumerable<BookInfo> GetBooks()
        {
            using (LibUnitOfWork uow = new LibUnitOfWork())
            {
                return uow.BookInfoRepository.GetAll();
            }
        }

        public void DeleteBook(BookInfo bookInfo)
        {
            using (LibUnitOfWork uow = new LibUnitOfWork())
            {
                var _bookInfo = uow.BookInfoRepository.FindById(bookInfo.Id);
                uow.BookInfoRepository.Remove(_bookInfo);
                uow.SaveChanges();
            }
        }

        public BookInfo BookInfoOf(BookCopy bookCopy)
        {
            using (var bookInfoRepo = new BookInfoRepository())
            {
                return bookInfoRepo.BookInfoOf(bookCopy);
            }
        }

        public IEnumerable<string> GetDistinctSubjects()
        {
            using (var bookInfoRepo = new BookInfoRepository())
            {
                return bookInfoRepo.GetAll().Select(b => b.Subject).Distinct();
            }
        }
    }
}
