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
    public class AuthenticationViewModel : ViewModelBase
    {
        private LibraryUser _CurrentLibraryUser = null;
        private string _Username;
        private readonly LibraryUserService _LibraryUserService;
        private RelayCommand _LoginCommand;
        private RelayCommand _LogoutCommand;
        private string _Password;

        public bool HasAuthenticatedUser
        {
            get { return _CurrentLibraryUser != null; }
        }

        public LibraryUser CurrentLibraryUser
        {
            get { return _CurrentLibraryUser; }
            set
            {
                _CurrentLibraryUser = value;

                RaisePropertyChanged(() => LoginCommand);
                RaisePropertyChanged(() => LogoutCommand);
                RaisePropertyChanged(() => HasAuthenticatedUser);
            }
        }

        public RelayCommand LoginCommand
        {
            get
            {
                return _LoginCommand = _LoginCommand ?? new RelayCommand(() =>
                {
                    CurrentLibraryUser = _LibraryUserService.GetAuthenticatedUser(Username, Password);

                }, () => IsInputFilledUp());
            }

        }

        public RelayCommand LogoutCommand
        {
            get
            {
                return _LogoutCommand = _LogoutCommand ?? new RelayCommand(() =>
                {
                    CurrentLibraryUser = null;
                }, () => HasAuthenticatedUser);
            }

        }

        private bool IsInputFilledUp()
        {
            return string.IsNullOrEmpty(Username) == false && string.IsNullOrEmpty(Password) == false;
        }


        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                RaisePropertyChanged(() => Password);
            }
        }


        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        /// <summary>
        /// Initializes a new instance of the LoginPageViewModel class.
        /// </summary>
        public AuthenticationViewModel(LibraryUserService libUserService)
        {
            _LibraryUserService = libUserService;
        }

    }
}