using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using ritchell.library.model.Interfaces;

namespace ritchell.library.model.Repositories
{
    public class BookInfoRepository : RepositoryBase<BookInfo>, IBookInfoRepository
    {
        public BookInfoRepository(System.Data.Entity.DbContext _Context)
            : base(_Context)
        { }

        public BookInfoRepository() : this(new LibraryContext())
        { }

        public int GetNumberOfCopies(BookInfo bookInfo)
        {
            return _Context.Set<BookCopy>().Where(b => b.BookInfoId == bookInfo.Id).Count();
        }

        public int GetNumberOfAvailableCopies(BookInfo bookInfo)
        {
            return _Context.Set<BookCopy>().Where(b => b.BookInfoId == bookInfo.Id && b.IsBorrowed == false).ToList().Count();
        }

        public BookInfo BookInfoOf(BookCopy bookCopy)
        {
            return _Context.Set<BookInfo>().Where(b => b.Id.Equals(bookCopy.BookInfoId)).SingleOrDefault();
        }

        public IEnumerable<BookInfo> SearchForBooks(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _Context.Set<BookInfo>().ToList();
            else
            {
                var lowered_keyword = keyword.ToLower();

                return _Context.Set<BookInfo>().Where(b => b.BookTitle.ToLower().Contains(lowered_keyword) ||
                    b.Author.ToLower().Contains(lowered_keyword) || b.Copyright.ToString().ToLower().Contains(lowered_keyword) ||
                    b.Subject.ToLower().Contains(lowered_keyword)).ToList();
            }
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
        // ~BookInfoRepository() {
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
