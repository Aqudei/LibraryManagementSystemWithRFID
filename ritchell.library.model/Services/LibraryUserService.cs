using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RijndaelEncryptDecrypt;
using ritchell.library.model.Repositories;
using RijndaelEncryptDecrypt;

namespace ritchell.library.model.Services
{
    public class LibraryUserService
    {
        public LibraryUser FindUserByUsernameAndPassword(string username, string password)
        {
            using (var uow = new LibUnitOfWork())
            {
                var user = uow.LibraryUserRepository.FindByUsername(username);

                if (user == null)
                    return null;

                if (user.Password == password)
                    return user;

                return null;
            }
        }

        public LibraryUser FindUserByRFID(string userTag)
        {
            using (var uow = new LibUnitOfWork())
            {
                return uow.LibraryUserRepository.FindByUserRFIDTag(userTag);
            }
        }

    }
}
