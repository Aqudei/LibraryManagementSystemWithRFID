using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RijndaelEncryptDecrypt;
using ritchell.library.model.Repositories;

namespace ritchell.library.model.Services
{
    public class LibraryUserService
    {
        public LibraryUser GetAuthenticatedUser(string username, string password)
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

        public LibraryUser GetAuthenticatedAdmin(string username, string password)
        {
            var libUser = GetAuthenticatedUser(username, password);
            if (libUser.LibraryUserType == LibraryUser.UserType.Admin)
                return libUser;
            else
                return null;
        }

        public LibraryUser FindUserByRFID(string userTag)
        {
            using (var uow = new LibUnitOfWork())
            {
                return uow.LibraryUserRepository.FindByUserRFIDTag(userTag);
            }
        }

        public void AddLibraryUser(LibraryUser user)
        {
            using (var uow = new LibUnitOfWork())
            {
                uow.LibraryUserRepository.Add(user);
                uow.SaveChanges();
            }
        }

        public void AddOrUpdateLibraryUser(LibraryUser libraryUser)
        {
            using (LibUnitOfWork uow = new LibUnitOfWork())
            {
                var libUser = uow.LibraryUserRepository.FindById(libraryUser.Id);

                if (libUser == null)
                    uow.LibraryUserRepository.Add(libUser);
                else
                    uow.LibraryUserRepository.Update(libUser);

                uow.SaveChanges();
            }
        }
    }
}
