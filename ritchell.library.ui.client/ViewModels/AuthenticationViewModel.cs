using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ritchell.library.model;
using ritchell.library.model.Repositories;
using ritchell.library.model.Services;
using ritchell.library.ui.client.ViewModels.VMMessages;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

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
        private string _Username;
        private string _Password;
        private RelayCommand _LoginCommand;
        private RelayCommand _LogoutCommand;
        private readonly LibraryUserService _LibraryUserService;
        private bool _IsAuthenticated;

        public RelayCommand LoginCommand
        {
            get
            {
                return _LoginCommand = _LoginCommand ?? new RelayCommand(() =>
                {
                    var currentLibraryUser = _LibraryUserService.GetAuthenticatedUser(Username, Password);

                    if (currentLibraryUser != null && currentLibraryUser.LibraryUserType != LibraryUser.UserType.Admin)
                    {
                        CurrentUser = currentLibraryUser;
                        MessageToUser = "Welcome " + currentLibraryUser.Fullname;
                        RaiseUserEvent(UserEvent.UserEventType.Login, CurrentUser);
                        IsAuthenticated = true;
                    }
                    else {
                        IsAuthenticated = false;
                        CurrentUser = null;
                        DialogService.ShowMessage("Invalid Username/Password\nPlease try again...", "");
                    }

                }, () => IsInputFilledUp() && !IsAuthenticated);
            }
        }

        private IDialogService _DialogService;


        private void RaiseUserEvent(UserEvent.UserEventType userEventType, LibraryUser libUser)
        {
            var handler = LibraryUserEventHandler;
            if (handler != null)
            {
                handler(this, new UserEvent { LibraryUser = libUser, LibraryUserEventType = userEventType });
            }
        }

        public event EventHandler<UserEvent> LibraryUserEventHandler;

        public RelayCommand LogoutCommand
        {
            get
            {
                return _LogoutCommand = _LogoutCommand ?? new RelayCommand(() =>
                {
                    Username = "";
                    Password = "";
                    MessageToUser = "";
                    IsAuthenticated = false;
                    RaiseUserEvent(UserEvent.UserEventType.Login, CurrentUser);
                }, () => IsAuthenticated);
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
            IsAuthenticated = false;
        }

        private string _UserToMessage;


        public string MessageToUser
        {
            get { return _UserToMessage; }
            set
            {
                _UserToMessage = value;
                RaisePropertyChanged(() => MessageToUser);
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _IsAuthenticated;
            }

            set
            {
                _IsAuthenticated = value;
                RaisePropertyChanged(() => IsAuthenticated);
                RaisePropertyChanged(() => IsNotAuthenticated);
            }
        }

        public bool IsNotAuthenticated
        {
            get { return !IsAuthenticated; }
            set
            {
                IsAuthenticated = !value;
                RaisePropertyChanged(() => IsNotAuthenticated);
                RaisePropertyChanged(() => IsAuthenticated);
            }
        }

        public LibraryUser CurrentUser { get; internal set; }

        public IDialogService DialogService
        {
            get
            {
                return _DialogService = _DialogService ?? SimpleIoc.Default.GetInstance<IDialogService>();
            }
        }
    }
}