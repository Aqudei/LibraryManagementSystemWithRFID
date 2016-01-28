using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using ritchell.library.model.Services;
using ritchell.library.presentation.common.ViewServices;
using ritchell.library.ui.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.ui.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private RelayCommand _LoginCommand;
        private LibraryUserService _UserService;
        private IWindowNavigationService _WindowNav;
        private string _Message;
        private IDialogService _DialogService;

        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Gets the LoginCommand.
        /// </summary>
        public RelayCommand LoginCommand
        {
            get
            {
                return _LoginCommand
                    ?? (_LoginCommand = new RelayCommand(
                    () =>
                    {
                        var admin = _UserService.GetAuthenticatedAdmin(Username, Password);
                        if (admin != null)
                        {
                            _WindowNav.ShowDialog(WindowNames.MainWindow, null);
                            MessengerInstance.Send("close");
                        }
                        else
                        {
                            Message = "Incorrect username or password";
                            _DialogService.ShowMessageBox(Message, "Authentication Failed");
                        }
                    }, () => InputFieldUp()));
            }
        }

        public LoginViewModel(LibraryUserService userService,
            IWindowNavigationService windowNav, IDialogService dialogService)
        {
            _UserService = userService;
            _WindowNav = windowNav;
            _DialogService = dialogService;
        }

        private bool InputFieldUp()
        {
            return string.IsNullOrEmpty(Username) == false && string.IsNullOrEmpty(Password) == false;
        }
    }
}
