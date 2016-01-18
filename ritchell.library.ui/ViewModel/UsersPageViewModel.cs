using System;
using GalaSoft.MvvmLight;
using ritchell.library.model;
using ritchell.library.model.Services;
using ritchell.library.model.Repositories;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        private ObservableCollection<Department> _DepartmentSource;
        private string _PasswordCopy;
        private DepartmentService _DepartmentService;

        /// <summary>
        /// Initializes a new instance of the UsersPageViewModel class.
        /// </summary>
        public UsersPageViewModel(LibraryUserService libraryUserService,
            DepartmentService departmentService)
        {
            _LibraryUserService = libraryUserService;
            _DepartmentService = departmentService;
            using (var repo = new LibraryUserRepository())
            {
                items = new System.Collections.ObjectModel.ObservableCollection<LibraryUser>(repo.GetAll());
                ItemsCollectionView = (ICollectionView)CollectionViewSource.GetDefaultView(items);
            }

            DepartmentSource = new ObservableCollection<Department>(_DepartmentService.GetDepartments());
        }

        public override void DeleteItemCommandHandler()
        {
            var currentUser = ItemsCollectionView.CurrentItem as LibraryUser;
            if (currentUser != null)
            {
                _LibraryUserService.DeleteUser(currentUser);
                items.Remove(currentUser);
            }
        }

        protected override void NewItemCommandHandler()
        {
            LibraryUser user = new LibraryUser();
            items.Add(user);
            ItemsCollectionView.MoveCurrentTo(user);
            ItemsCollectionView.Refresh();
        }

        public override bool InputFieldsAreValid()
        {
            var current = ItemsCollectionView.CurrentItem as LibraryUser;
            if (current == null)
                return false;

            if (string.IsNullOrEmpty(current.Password))
                return false;

            if (string.IsNullOrEmpty(PasswordCopy))
                return false;

            return current.Password == PasswordCopy;
        }


        public string PasswordCopy
        {
            get { return _PasswordCopy; }
            set
            {
                _PasswordCopy = value;
                RaisePropertyChanged(() => PasswordCopy);
                SaveItemCommand.RaiseCanExecuteChanged();
            }
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


        public ObservableCollection<Department> DepartmentSource
        {
            get
            {
                return _DepartmentSource;
            }
            set
            {
                _DepartmentSource = value;
                RaisePropertyChanged(() => DepartmentSource);
            }
        }
    }
}