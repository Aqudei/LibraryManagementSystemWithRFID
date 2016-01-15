using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.model.LibraryTransactions;
using ritchell.library.model.Services;

namespace ritchell.library.ui.client.ViewModels
{
    public class BorrowReturnBookViewModel : ViewModelBase
    {
        private LibraryTransactionsAggregate _BatchLibraryTransactions;
        private RelayCommand _ResetTransactionsCommand;
        private RelayCommand _ProceedCommand;
        private RelayCommand _PayFeeCommand;
        private LibraryUserService _LibraryUserService;
        private string _Username;
        private string _Password;

        public BorrowReturnBookViewModel(LibraryUserService libUservice)
        {
            _LibraryUserService = libUservice;

            var user = SimpleIoc.Default.GetInstance<AuthenticationViewModel>().CurrentLibraryUser;

            BatchLibraryTransactions = new LibraryTransactionsAggregate(user);
        }

        public RelayCommand ResetTransactionsCommand
        {
            get
            {
                return _ResetTransactionsCommand = _ResetTransactionsCommand ?? new RelayCommand(
                    () =>
                    {
                        BatchLibraryTransactions.Reset();
                    },
                    () => BatchLibraryTransactions != null);
            }
        }

        /// <summary>
        /// Gets the ProceedCommand.
        /// </summary>
        public RelayCommand ProceedCommand
        {
            get
            {
                return _ProceedCommand
                    ?? (_ProceedCommand = new RelayCommand(
                    () =>
                    {
                        BatchLibraryTransactions.ExecuteAll();
                    },
                    () => BatchLibraryTransactions != null &&
                    BatchLibraryTransactions.RequiredFee <= 0));
            }
        }

        public RelayCommand PayFeeCommand
        {
            get
            {
                return _PayFeeCommand = _PayFeeCommand ?? new RelayCommand(() =>
                {
                    BatchLibraryTransactions.PayNecessaryFees(Username, Password);
                }, () => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password));
            }
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

        public LibraryTransactionsAggregate BatchLibraryTransactions
        {
            get
            {
                return _BatchLibraryTransactions;
            }

            set
            {
                _BatchLibraryTransactions = value;
            }
        }
    }
}
