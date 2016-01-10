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


        private RelayCommand _LoginCommand;

        public RelayCommand LoginCommand
        {
            get
            {
                return _LoginCommand = _LoginCommand ?? new RelayCommand(() =>
                {
                    CurrentLibraryUser = _LibraryUserService.GetAuthenticatedUser(Username, Password);

                }, () => InputFilledUp());
            }

        }

        private bool InputFilledUp()
        {
            return string.IsNullOrEmpty(Username) == false && string.IsNullOrEmpty(Password);
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        private void AuthenticateUser()
        {

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