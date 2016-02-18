using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using ritchell.library.model.Interfaces;

namespace ritchell.library.model.Repositories
{
    public class BookCopyRepository : RepositoryBase<BookCopy>, IBookCopyRepository
    {
        public BookCopyRepository(System.Data.Entity.DbContext context)
            : base(context)
        { }

        public BookCopyRepository() : this(new LibraryContext())
        { }

        public BookCopy FindByShortRangeRFId(string shortRangeRFId)
        {
            return _Context.Set<BookCopy>().Where(b => b.BookTagShort == shortRangeRFId).FirstOrDefault();
        }

        public BookCopy FindByLongRangeRFId(string longRangeRFId)
        {
            return _Context.Set<BookCopy>().Where(b => b.BookTagLong == longRangeRFId).FirstOrDefault();
        }

        public BookInfo BookInfoOf(BookCopy bookCopy)
        {
            return _Context.Set<BookInfo>().Where(b => b.Id == bookCopy.BookInfoId).FirstOrDefault();
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
        // ~BookCopyRepository() {
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


        #endregion IDisposable Support

    }
}
