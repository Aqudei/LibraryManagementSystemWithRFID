using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace ritchell.library.ui.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BorrowedBooksManager : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the BorrowedBooksManager class.
        /// </summary>
        public BorrowedBooksManager()
        {
        }

        private RelayCommand _ReturnApplyPayment;

        /// <summary>
        /// Gets the ReturnApplyPaymentCommand.
        /// </summary>
        public RelayCommand ReturnApplyPaymentCommand
        {
            get
            {
                return _ReturnApplyPayment
                    ?? (_ReturnApplyPayment = new RelayCommand(
                    () =>
                    {

                    },
                    () => true));
            }
        }
    }
}