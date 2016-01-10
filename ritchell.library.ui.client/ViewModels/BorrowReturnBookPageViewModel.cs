using GalaSoft.MvvmLight;
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
    public class BorrowReturnBookPageViewModel : ViewModelBase
    {
        ObservableCollection<ILibraryTransaction> _TransactionList;

        public ObservableCollection<ILibraryTransaction> TransactionList
        {
            get
            {
                return _TransactionList = _TransactionList ?? new ObservableCollection<ILibraryTransaction>();
            }

            set
            {
                _TransactionList = value;
                RaisePropertyChanged(() => TransactionList);
            }
        }
    }
}
