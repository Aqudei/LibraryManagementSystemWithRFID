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
        public static LibraryTransactionBase CreateTransaction(LibraryUser libraryUser, string bookTag)
        {
            using (var uow = new LibUnitOfWork())
            {
                var bookCopy = uow.BookCopyRepository.FindByShortRangeRFId(bookTag);

                if (bookCopy == null)
                    throw new InvalidOperationException("Unknown Book!");

                if (bookCopy.IsBorrowed == true)
                {
                    var lastBookTrans = uow.BookTransactionInfoRepository.GetLastBookTransaction(bookCopy.Id);

                    if (lastBookTrans == null)
                        throw new InvalidOperationException("The book has no known borrowed information.");

                    else if (libraryUser.LibraryUserType == LibraryUser.UserType.Teacher)
                        return new ReturnBookIgnorePaymentTransaction(libraryUser, bookCopy, lastBookTrans);
                    else if (libraryUser.Id != lastBookTrans.LibraryUserId)
                        throw new InvalidOperationException("Please surrender this book to admin");
                    else
                        return new ReturnBookTransaction(libraryUser, bookCopy, lastBookTrans);
                }
                else
                {
                    return new BorrowBookTransaction(libraryUser, bookCopy);
                }
            }
        }
    }
}
