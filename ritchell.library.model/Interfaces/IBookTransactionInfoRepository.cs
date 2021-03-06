﻿using ritchell.library.infrastructure;
using ritchell.library.model.LibraryTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.Interfaces
{
    public interface IBookTransactionInfoRepository : IRepository<TransactionInfo>, IDisposable
    {
        TransactionInfo GetLastBookTransaction(Guid BookCopyId);
        IEnumerable<TransactionInfo> GetPayableTransactions(Guid UserId);
        IEnumerable<TransactionInfo> GetTransactionsOf(Guid UserId);
    }
}
