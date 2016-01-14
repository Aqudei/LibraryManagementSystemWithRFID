using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.model.Services;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace ritchell.library.ui.client.ViewModels
{
    public class AdminRequiredViewModel : ViewModelBase
    {
        private LibraryUserService _LibraryUserService;

        public AdminRequiredViewModel(LibraryUserService libuserservice)
        {
            _LibraryUserService = libuserservice;
        }

        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private RelayCommand _AdminVerifyCommand;

        /// <summary>
        /// Gets the AdminVerifyCommand.
        /// </summary>
        public RelayCommand AdminVerifyCommand
        {
            get
            {
                return _AdminVerifyCommand
                    ?? (_AdminVerifyCommand = new RelayCommand(
                    () =>
                    {
                        var admin = _LibraryUserService.GetAuthenticatedAdmin(Username, Password);
                        if (admin != null)
                            Messenger.Default.Send<VMMessages.UserSuccessfullyAuthenticated>(new VMMessages.UserSuccessfullyAuthenticated { AuthenticatedUser = admin });
                    },
                    () => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password)));
            }
        }

        private RelayCommand _CancelVerifyAdmin;

        /// <summary>
        /// Gets the CancelVerifyAdmin.
        /// </summary>
        public RelayCommand CancelVerifyAdmin
        {
            get
            {
                return _CancelVerifyAdmin
                    ?? (_CancelVerifyAdmin = new RelayCommand(
                    () =>
                    {

                    },
                    () => true));
            }
        }
    }
}
