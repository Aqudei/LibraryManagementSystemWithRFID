using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.model;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ritchell.library.ui.client.ViewModels
{
    public class TransactionsViewModel : ViewModelBase
    {
        private LibraryUser _LibraryUser;
        ICollectionView _TransactionsView;

        public TransactionsViewModel(LibraryUser libUser)
        {
            _LibraryUser = libUser;
            TransactionsView = CollectionViewSource
                .GetDefaultView(new ObservableCollection<TransactionDTO>(TranService.GetTransactionDTO(libUser.Id)));

            TransactionsView.SortDescriptions.Add(new SortDescription("TransactionInfo.BorrowDate", ListSortDirection.Descending));
        }

        public ICollectionView TransactionsView
        {
            get
            {
                return _TransactionsView;
            }

            set
            {
                _TransactionsView = value;
                RaisePropertyChanged(() => TransactionsView);
            }
        }

        private TransactionService TranService
        {
            get { return SimpleIoc.Default.GetInstance<TransactionService>(); }

        }
    }
}
