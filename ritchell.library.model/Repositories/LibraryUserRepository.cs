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
    }
}
