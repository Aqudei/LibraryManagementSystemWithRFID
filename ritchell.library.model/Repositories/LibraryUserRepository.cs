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
    public class LibraryUserRepository : RepositoryBase<LibraryUser>, ILibraryUserRepository
    {
        public LibraryUserRepository(DbContext context)
            : base(context)
        { }

        public LibraryUserRepository() : this(new LibraryContext())
        { }

        public LibraryUser FindByUsernameAndEncryptedPassword(string username, string encPass)
        {
            return _Context.Set<LibraryUser>()
                .Where(u => u.Username == username && u.EncryptedPassword == encPass)
                .FirstOrDefault();
        }

        public LibraryUser FindByUserRFIDTag(string userRFIDTag)
        {
            return _Context.Set<LibraryUser>()
                .Where(u => u.UserRFIDTag == userRFIDTag).FirstOrDefault();
        }


        public LibraryUser FindByUsername(string username)
        {
            return _Context.Set<LibraryUser>()
                .Where(u => u.Username == username).FirstOrDefault();
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
                    this._Context.Dispose();
                    this._Context = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LibraryUserRepository() {
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
