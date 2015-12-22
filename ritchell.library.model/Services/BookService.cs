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
        public void EnrollOrUpdateBook(BookInfo bookInfo)
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
    }
}
