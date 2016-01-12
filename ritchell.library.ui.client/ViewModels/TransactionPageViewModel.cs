using GalaSoft.MvvmLight;
using ritchell.library.model.LibraryTransactions;
using System.Collections.ObjectModel;

namespace ritchell.library.ui.client.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TransactionPageViewModel : ViewModelBase
    {

        private ObservableCollection<LibraryTransactionBase> _LibraryTransactions;

        public ObservableCollection<LibraryTransactionBase> LibraryTransactions
        {
            get { return _LibraryTransactions; }
            set
            {
                _LibraryTransactions = value;
                RaisePropertyChanged(() => LibraryTransactions);
            }
        }

        /// <summary>
        /// Initializes a new instance of the TransactionPageViewModel class.
        /// </summary>
        public TransactionPageViewModel()
        {
            LibraryTransactions = new ObservableCollection<LibraryTransactionBase>();
        }
    }
}