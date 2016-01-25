using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ritchell.library.model.Services;
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

                        }
                        else
                        {

                        }
                    }, () => InputFieldUp()));
            }
        }

        public LoginViewModel(LibraryUserService userService)
        {
            _UserService = userService;
        }

        private bool InputFieldUp()
        {
            return string.IsNullOrEmpty(Username) == false && string.IsNullOrEmpty(Password) == false;
        }
    }
}
