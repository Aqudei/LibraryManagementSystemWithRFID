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
    public class PaymentViewModel : ViewModelBase
    {
        private LibraryUserService _LibraryUserService;
        private string _Username;
        private string _Password;
        private RelayCommand _AdminVerifyCommand;
        private RelayCommand _CancelVerifyAdmin;

        public PaymentViewModel(LibraryUserService libuserservice)
        {
            _LibraryUserService = libuserservice;
        }

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

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
                            Messenger.Default.Send<VMMessages.UserSuccessfullyPay>(new VMMessages.UserSuccessfullyPay { AuthenticatedUser = admin });
                    },
                    () => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password)));
            }
        }

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
