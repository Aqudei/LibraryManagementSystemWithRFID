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
        public IEnumerable<BookSearchResultDTO> Search(string keyword)
        {
            List<BookSearchResultDTO> bookSearchDTO = new List<BookSearchResultDTO>();
            using (var sectionRepo = new SectionRepository())
            using (var bookInfoRepo = new BookInfoRepository())
            {
                var books = bookInfoRepo.SearchForBooks(keyword);

                foreach (var book in books)
                {
                    var section = sectionRepo.GetBookSection(book);
                    var booksearch = new BookSearchResultDTO
                    {
                        BookInfo = book,
                        Section = section
                    };
                    bookSearchDTO.Add(booksearch);
                }

                return bookSearchDTO;
            }
        }
    }
}
