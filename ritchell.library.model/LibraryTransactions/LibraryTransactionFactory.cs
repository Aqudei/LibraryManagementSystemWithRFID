using ritchell.library.model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class LibraryTransactionFactory
    {
        public static LibraryTransactionBase CreateTransaction(Guid libUserId, string bookTag)
        {
            using (var bookCopyRepo = new BookCopyRepository())
            {
                var bookCopy = bookCopyRepo.FindByShortRangeRFId(bookTag);

                if (bookCopy == null)
                    throw new InvalidOperationException("Unknown Book!");

                if (bookCopy.IsBorrowed == true)
                {
                    return new ReturnBookTransaction(libUserId, bookTag);
                }
                else
                {
                    return new BorrowBookTransaction(libUserId, bookTag);
                }
            }
        }
    }
}
