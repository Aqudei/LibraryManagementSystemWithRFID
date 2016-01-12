using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.model;
using ritchell.library.model.LibraryTransactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.ui.client.ViewModels
{
    public class BorrowReturnBookViewModel : ViewModelBase
    {
        private BatchLibraryTransactions _BatchLibraryTransactions;
        private RelayCommand _ResetTransactionsCommand;

        public BorrowReturnBookViewModel()
        {
            var user = SimpleIoc.Default.GetInstance<AuthenticationViewModel>().CurrentLibraryUser;

            BatchLibraryTransactions = new BatchLibraryTransactions(user);
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

        private RelayCommand _ProceedCommand;

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
                    () => BatchLibraryTransactions != null));
            }
        }

        public BatchLibraryTransactions BatchLibraryTransactions
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
