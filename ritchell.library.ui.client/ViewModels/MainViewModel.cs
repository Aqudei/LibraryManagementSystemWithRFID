using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.model.LibraryTransactions;
using ritchell.library.infrastructure.Hardware;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using System.Diagnostics;
using ritchell.library.ui.client.ViewServices;
using GalaSoft.MvvmLight.Views;
using ritchell.library.ui.client.ViewModels.VMMessages;

namespace ritchell.library.ui.client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private RelayCommand _ClearTransactionsCommand;
        private IWindowNavigationService _WindowNaviService;
        private IDialogService _DialogService;
        private AuthenticationViewModel _AuthenticationViewModel;
        private LibraryTransactionsAggregate _LibraryTransactionsAggregate;
        private RelayCommand _PayNowCommand;
        private RelayCommand _ProceedCommand;
        private string _AdminUsername;
        private string _AdminPassword;

        private RelayCommand<string> _OpenWindowCommand;

        /// <summary>
        /// Gets the OpenWindowCommand.
        /// </summary>
        public RelayCommand<string> OpenWindowCommand
        {
            get
            {
                return _OpenWindowCommand
                    ?? (_OpenWindowCommand = new RelayCommand<string>(
                    (x) =>
                    {
                        if (x.ToLower().Contains("payment"))
                            _WindowNaviService.ShowPaymentsOf(AuthenticationViewModel.CurrentUser);
                    }));
            }
        }

        public AuthenticationViewModel AuthenticationViewModel
        {
            get
            {
                return _AuthenticationViewModel = _AuthenticationViewModel ?? SimpleIoc.Default.GetInstance<AuthenticationViewModel>();
            }
            set
            {
                _AuthenticationViewModel = value;
                RaisePropertyChanged(() => AuthenticationViewModel);
            }
        }

        public LibraryTransactionsAggregate LibraryTransactionsAggregate
        {
            get
            {
                return _LibraryTransactionsAggregate;
            }

            set
            {
                _LibraryTransactionsAggregate = value;
            }
        }

        public MainViewModel(IWindowNavigationService windowDlgService)
        {
            _WindowNaviService = windowDlgService;

            LibraryTransactionsAggregate = new LibraryTransactionsAggregate();

            var shortRfidReader = SimpleIoc.Default.GetInstance<IRFIDReader>("short");
            shortRfidReader.StartReader();
            AuthenticationViewModel.LibraryUserEventHandler += (s, e) =>
            {
                if (e.LibraryUserEventType == VMMessages.UserEvent.UserEventType.Login)
                {
                    LibraryTransactionsAggregate.PrepareForNewUser(e.LibraryUser);
                    shortRfidReader.TagRead += ShortRfidReader_TagRead;
                }
                else
                {
                    LibraryTransactionsAggregate.Reset();
                    shortRfidReader.TagRead -= ShortRfidReader_TagRead;
                }
            };

            MessengerInstance.Register<ApplicationExiting>(this, (x) =>
            {
                shortRfidReader.StopReader();
            });
        }

        private void ShortRfidReader_TagRead(object sender, string e)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                try
                {
                    LibraryTransactionsAggregate.AddTransaction(e);
                    //_DialogService.ShowMessage("Transaction Completed!", "");
                }
                catch (Exception ex)
                {
                    //_DialogService.ShowMessage(ex.Message, "");
                }

                RaisePropertyChanged(() => LibraryTransactionsAggregate.LibraryTransactions);
                ClearTransactionsCommand.RaiseCanExecuteChanged();
                ProceedWithTransactionCommand.RaiseCanExecuteChanged();
                PayNowCommand.RaiseCanExecuteChanged();
            });
        }


        public RelayCommand PayNowCommand
        {
            get
            {
                return _PayNowCommand = _PayNowCommand ?? new RelayCommand(() =>
                {
                    try
                    {
                        LibraryTransactionsAggregate.CompletePayment(AdminUsername, AdminPassword);
                        DialogService.ShowMessage("Payment Completed", "");
                    }
                    catch (Exception ex)
                    {
                        DialogService.ShowMessage(ex.Message, "");
                    }

                },
                () => LibraryTransactionsAggregate.RequiredFee > 0
                        && !string.IsNullOrEmpty(AdminUsername)
                        && !string.IsNullOrEmpty(AdminPassword));
            }
        }

        public RelayCommand ProceedWithTransactionCommand
        {
            get
            {
                return _ProceedCommand = _ProceedCommand ?? new RelayCommand(() =>
                {
                    try
                    {
                        foreach (var transaction in LibraryTransactionsAggregate.LibraryTransactions)
                        {
                            transaction.Execute();
                        }
                        DialogService.ShowMessageBox("Transaction(s) Completed...", "");
                    }
                    catch (Exception ex)
                    {
                        DialogService.ShowMessageBox(ex.Message, "");
                        Debug.WriteLine("{0} @ ProceedWithTransactionCommand", ex.Message);
                    }
                }, () => LibraryTransactionsAggregate.LibraryTransactions.Count > 0);
            }
        }

        public string AdminUsername
        {
            get { return _AdminUsername; }
            set
            {
                _AdminUsername = value;
                RaisePropertyChanged(() => AdminUsername);
            }
        }

        public string AdminPassword
        {
            get { return _AdminPassword; }
            set
            {
                _AdminPassword = value;
                RaisePropertyChanged(() => AdminPassword);
            }
        }

        public IDialogService DialogService
        {
            get
            {
                return _DialogService = SimpleIoc.Default.GetInstance<IDialogService>();
            }
        }

        public RelayCommand ClearTransactionsCommand
        {
            get
            {
                return _ClearTransactionsCommand = _ClearTransactionsCommand ?? new RelayCommand(() =>
                {
                    LibraryTransactionsAggregate.Reset();
                }, () => LibraryTransactionsAggregate != null && LibraryTransactionsAggregate.LibraryTransactions.Count > 0);
            }
        }
    }
}
