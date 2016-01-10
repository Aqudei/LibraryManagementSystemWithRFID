using System;
using GalaSoft.MvvmLight;
using ritchell.library.model;
using ritchell.library.model.Services;
using ritchell.library.model.Repositories;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;

namespace ritchell.library.ui.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class UsersPageViewModel : WithEditableItems<LibraryUser>
    {
        private readonly LibraryUserService _LibraryUserService;

        /// <summary>
        /// Initializes a new instance of the UsersPageViewModel class.
        /// </summary>
        public UsersPageViewModel(LibraryUserService libraryUserService)
        {

            _LibraryUserService = libraryUserService;
            using (var repo = new LibraryUserRepository())
            {
                items = new System.Collections.ObjectModel.ObservableCollection<LibraryUser>(repo.GetAll());
                ItemsCollectionView = (ICollectionView)CollectionViewSource.GetDefaultView(items);
            }
        }

        public override void DeleteItemCommandHandler()
        {
            using (var uow = new library.model.Repositories.LibUnitOfWork())
            {
                var currentUser = ItemsCollectionView.CurrentItem as LibraryUser;
                uow.LibraryUserRepository.Remove(currentUser);
                items.Remove(currentUser);
            }
        }

        protected override void NewItemCommandHandler()
        {
            LibraryUser user = new LibraryUser();
            items.Add(user);
            ItemsCollectionView.MoveCurrentTo(user);
        }

        protected override void SaveItemCommandHandler()
        {
            var currentUser = ItemsCollectionView.CurrentItem as LibraryUser;
            _LibraryUserService.AddOrUpdateLibraryUser(currentUser);
        }

        public override void EditItemCommandHandler()
        {

        }

        public Array UserTypes
        {
            get
            {
                return Enum.GetValues(typeof(LibraryUser.UserType));
            }
        }

    }
}