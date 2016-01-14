using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using ritchell.library.model.Interfaces;

namespace ritchell.library.model.Repositories
{
    public class SectionRepository : RepositoryBase<Section>, ISectionRepository
    {
        public SectionRepository() : this(new LibraryContext())
        { }

        public SectionRepository(DbContext context)
            : base(context)
        { }

        public int GetNumberOfBooks(Guid SectionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookInfo> GetBooks(Guid SectionId)
        {
            return _Context.Set<Section>()
                .Include(s => s.BookInfos)
                .Where(s => s.Id == SectionId)
                .Single().BookInfos;
        }

        public Section GetBookSection(BookInfo bookInfo)
        {
            return _Context.Set<Section>().Where(s => s.Id == bookInfo.SectionId).FirstOrDefault();
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
        // ~SectionRepository() {
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
    }
}
