﻿using ritchell.library.model.Repositories;
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
                        return new ReturnBookIgnorePaymentTransaction(lastBookTrans);
                    else
                        return new ReturnBookTransaction(lastBookTrans);
                }
                else
                {
                    return new BorrowBookTransaction(libraryUser.Id, bookCopy.Id);
                }
            }
        }
    }
}
