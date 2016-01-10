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
        private BatchLibraryTransactions _LibraryTransactions;

        public BorrowReturnBookViewModel()
        {
            var user = SimpleIoc.Default.GetInstance<AuthenticationViewModel>().CurrentLibraryUser;

            LibraryTransactions = new BatchLibraryTransactions(user);
        }

        public BatchLibraryTransactions LibraryTransactions
        {
            get
            {
                return _LibraryTransactions;
            }

            set
            {
                _LibraryTransactions = value;
            }
        }

        private RelayCommand _ResetTransactionsCommand;

        public RelayCommand ResetTransactionsCommand
        {
            get
            {
                return _ResetTransactionsCommand = _ResetTransactionsCommand ?? new RelayCommand(
                    () =>
                    {
                        LibraryTransactions.Reset();
                    },
                    () => LibraryTransactions != null);
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
                        LibraryTransactions.ExecuteAll();
                    },
                    () => LibraryTransactions != null));
            }
        }

    }
}
