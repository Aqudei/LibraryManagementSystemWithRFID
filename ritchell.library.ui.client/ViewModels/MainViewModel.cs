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

namespace ritchell.library.ui.client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private AuthenticationViewModel _AuthenticationViewModel;
        private LibraryTransactionsAggregate _LibraryTransactionsAggregate;

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

        public MainViewModel()
        {
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

        private RelayCommand _PayNowCommand;

        public RelayCommand PayNowCommand
        {
            get
            {
                return _PayNowCommand = _PayNowCommand ?? new RelayCommand(() =>
                {
                    LibraryTransactionsAggregate.CompletePayment(AdminUsername, AdminPassword);
                }, () => LibraryTransactionsAggregate.RequiredFee > 0);
            }
        }


        private RelayCommand _ProceedCommand;

        public RelayCommand ProceedWithTransactionCommand
        {
            get
            {
                return _ProceedCommand = _PayNowCommand ?? new RelayCommand(() =>
                {
                    try
                    {
                        foreach (var transaction in LibraryTransactionsAggregate.LibraryTransactions)
                        {
                            transaction.Execute();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("{0} @ ProceedWithTransactionCommand", ex.Message);
                    }
                }, () => true);
            }
        }


        private string _AdminUsername;

        public string AdminUsername
        {
            get { return _AdminUsername; }
            set
            {
                _AdminUsername = value;
                RaisePropertyChanged(() => AdminUsername);
            }
        }

        private string _AdminPassword;

        public string AdminPassword
        {
            get { return _AdminPassword; }
            set
            {
                _AdminPassword = value;
                RaisePropertyChanged(() => AdminPassword);
            }
        }



        RelayCommand _ClearTransactionsCommand;
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
