using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;

namespace ritchell.library.model.Interfaces
{
    public interface ILibraryUserRepository : IRepository<LibraryUser>
    {
        LibraryUser FindByUsernameAndEncryptedPassword(string username, string encPass);

        LibraryUser FindByUserRFIDTag(string userRFIDTag);

        LibraryUser FindByUsername (string username);
    }
}
