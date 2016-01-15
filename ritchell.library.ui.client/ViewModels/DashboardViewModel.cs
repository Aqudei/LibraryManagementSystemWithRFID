using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;

namespace ritchell.library.ui.client.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DashboardViewModel : ViewModelBase
    {

        private RelayCommand _ByBooksCommand;
        private RelayCommand _BorrowReturnBookCommand;
        private ViewModelBase _ContentRegion;
        private RelayCommand _LogoutCommand;

        /// <summary>
        /// Initializes a new instance of the DashboardPageViewModel class.
        /// </summary>
        public DashboardViewModel()
        {
        }

        public RelayCommand ByBooksCommand
        {
            get
            {
                return _ByBooksCommand = _ByBooksCommand ?? new RelayCommand(() =>
                {

                }, () => true);
            }
        }

        public RelayCommand BorrowReturnBookCommand
        {
            get
            {
                return _BorrowReturnBookCommand = _BorrowReturnBookCommand ?? new RelayCommand(() =>
                {
                    DashboardContent = SimpleIoc.Default.GetInstance<BorrowReturnBookViewModel>();
                }, () => true);
            }
        }

        public ViewModelBase DashboardContent
        {
            get
            {
                return _ContentRegion;
            }

            set
            {
                _ContentRegion = value;
                RaisePropertyChanged(() => DashboardContent);
            }
        }

        public RelayCommand LogoutCommand
        {
            get
            {
                return _LogoutCommand = _LogoutCommand ?? new RelayCommand(() => { },
              () => true);
            }
        }
    }
}