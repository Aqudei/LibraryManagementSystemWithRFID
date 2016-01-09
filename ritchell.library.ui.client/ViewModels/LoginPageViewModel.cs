using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ritchell.library.model;
using ritchell.library.model.Repositories;
using ritchell.library.model.Services;

namespace ritchell.library.ui.client.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoginPageViewModel : ViewModelBase
    {
        private LibraryUser _CurrentLibraryUser;

        private bool HasAuthenticatedUser()
        {
            return _CurrentLibraryUser != null;
        }

        public LibraryUser CurrentLibraryUser
        {
            get { return _CurrentLibraryUser; }
            set
            {
                _CurrentLibraryUser = value;
                RaisePropertyChanged(() => CurrentLibraryUser);
            }
        }


        private RelayCommand _BorrowCommand;

        public RelayCommand BorrowCommand
        {
            get
            {
                return _BorrowCommand = _BorrowCommand ?? new RelayCommand(() =>
                {

                }, () => HasAuthenticatedUser());
            }

        }


        private RelayCommand _ReturnCommand;

        public RelayCommand ReturnCommand
        {
            get
            {
                return _ReturnCommand = _ReturnCommand ?? new RelayCommand(() =>
                {

                }, () => HasAuthenticatedUser());
            }

        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                RaisePropertyChanged(() => Password);
                AuthenticateUser();
            }
        }

        private void AuthenticateUser()
        {
            CurrentLibraryUser = _LibraryUserService.GetAuthenticatedUser(Username, Password);
        }

        private string _Username;
        private readonly LibraryUserService _LibraryUserService;

        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                RaisePropertyChanged(() => Username);
                AuthenticateUser();
            }
        }

        /// <summary>
        /// Initializes a new instance of the LoginPageViewModel class.
        /// </summary>
        public LoginPageViewModel(LibraryUserService libUserService)
        {
            _LibraryUserService = libUserService;
        }


    }
}