using ritchell.library.model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.Services
{
    public class BookSearchService
    {

        public BookSearchService()
        {

        }

        public IEnumerable<BookSearchResultDTO> Search()
        {
            return Search("");
        }

        public IEnumerable<BookSearchResultDTO> Search(string keyword)
        {
            List<BookSearchResultDTO> bookSearchDTO = new List<BookSearchResultDTO>();

            using(var uow = new LibUnitOfWork())
            {
                var books = uow.BookInfoRepository.SearchForBooks(keyword);

                foreach (var book in books)
                {
                    var section = uow.SectionRepository.GetBookSection(book);
                    var booksearch = new BookSearchResultDTO
                    {
                        BookInfo = book,
                        Section = section
                    };
                    booksearch.AvailableCopies = uow.BookInfoRepository.GetNumberOfAvailableCopies(book);
                    booksearch.NumberOfCopies = uow.BookInfoRepository.GetNumberOfCopies(book);
                    bookSearchDTO.Add(booksearch);
                }

                return bookSearchDTO;
            }
        }
    }
}
