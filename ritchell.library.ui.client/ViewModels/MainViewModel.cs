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
        }

        private void ShortRfidReader_TagRead(object sender, string e)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => LibraryTransactionsAggregate.AddTransaction(e));
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
                        _DialogService.ShowMessage("Payment Completed", "");
                    }
                    catch (Exception ex)
                    {
                        _DialogService.ShowMessage(ex.Message, "");
                    }

                }, () => LibraryTransactionsAggregate.RequiredFee > 0);
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
                        DialogService.ShowMessage("Transaction(s) Completed...","");
                    }
                    catch (Exception ex)
                    {
                        DialogService.ShowMessage(ex.Message,"");
                        Debug.WriteLine("{0} @ ProceedWithTransactionCommand", ex.Message);
                    }
                }, () => true);
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
