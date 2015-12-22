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
        public void AddBookCopy(BookCopy bookCopy)
        {
            using (var uow = new LibUnitOfWork())
            {
                uow.BookCopyRepository.Add(bookCopy);
                uow.SaveChanges();
            }
        }

        
    }
}
