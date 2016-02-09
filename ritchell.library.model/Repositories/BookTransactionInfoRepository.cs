using ritchell.library.infrastructure;
using ritchell.library.model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ritchell.library.model.LibraryTransactions;

namespace ritchell.library.model.Repositories
{
    public class BookTransactionInfoRepository : RepositoryBase<TransactionInfo>, IBookTransactionInfoRepository
    {
        public BookTransactionInfoRepository(DbContext context) : base(context)
        { }

        public BookTransactionInfoRepository() : this(new LibraryContext())
        { }

        public TransactionInfo GetLastBookTransaction(Guid bookCopyId)
        {
            var bookTrans = _Context.Set<TransactionInfo>()
                .Where(t => t.BookCopyId.Equals(bookCopyId) && t.ReturnDate.HasValue == false)
                .ToList()
                .OrderBy(t => t.BorrowDate)
                .LastOrDefault();

            return bookTrans;
        }

        public IEnumerable<TransactionInfo> GetPayableTransactions(Guid UserId)
        {
            var bookTrans = _Context.Set<TransactionInfo>()
                 .Where(t => t.LibraryUserId.Equals(UserId) && t.IsPaid == false && t.AmountToPay > 0)
                .ToList().OrderBy(t => t.BorrowDate);

            return bookTrans;
        }

        public IEnumerable<TransactionInfo> GetReturnedBooksPayableTransactions()
        {
            var bookTrans = _Context.Set<TransactionInfo>()
                 .Where(t => (t.IsPaid == false && t.AmountToPay > 0))
                .ToList()
                .OrderBy(t => t.BorrowDate);

            return bookTrans;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _Context.Dispose();
                    _Context = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BookTransactionInfoRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }


        #endregion
        public IEnumerable<TransactionInfo> GetTransactionsOf(Guid UserId)
        {
            return _Context.Set<TransactionInfo>().Where(ti => ti.LibraryUserId == UserId).ToList();
        }

    }
}
