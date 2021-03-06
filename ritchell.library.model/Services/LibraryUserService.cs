﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RijndaelEncryptDecrypt;
using ritchell.library.model.Repositories;
using ritchell.library.model.LibraryTransactions;

namespace ritchell.library.model.Services
{
    public class LibraryUserService
    {
        public LibraryUser FindById(object id)
        {
            using (var userRepo = new LibraryUserRepository())
            {
                return userRepo.FindById(id);
            }
        }

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
            if (libUser == null)
                throw new InvalidOperationException("Invalid credentials");

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

        public void DeleteUser(LibraryUser currentUser)
        {
            using (var uow = new library.model.Repositories.LibUnitOfWork())
            {
                uow.LibraryUserRepository.Remove(currentUser);
                uow.SaveChanges();
            }
        }



        public void AddOrUpdateLibraryUser(LibraryUser libraryUser)
        {
            using (LibUnitOfWork uow = new LibUnitOfWork())
            {
                var libUser = uow.LibraryUserRepository.FindById(libraryUser.Id);

                var _libuser = uow.LibraryUserRepository.FindByUsername(libraryUser.Username);
                if (_libuser != null && _libuser.Id != libraryUser.Id)
                {
                    throw new InvalidOperationException("Username already exists");
                }

                if (libUser == null)
                    uow.LibraryUserRepository.Add(libraryUser);
                else
                    uow.LibraryUserRepository.Update(libraryUser);

                uow.SaveChanges();
            }
        }
    }
}
