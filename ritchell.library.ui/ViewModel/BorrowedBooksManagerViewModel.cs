using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ritchell.library.model;
using ritchell.library.model.LibraryTransactions;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.infrastructure.Hardware;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Threading;

namespace ritchell.library.ui.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BorrowedBooksManagerViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the BorrowedBooksManager class.
        /// </summary>
        public BorrowedBooksManagerViewModel(PaymentService payService)
        {
            _PaymentService = payService;
            RefreshList();
            SetupRFIDReader();
        }

        private void SetupRFIDReader()
        {
            _ShortRFIDReader = SimpleIoc.Default.GetInstance<IRFIDReader>("short");
            _ShortRFIDReader.TagRead += _ShortRFIDReader_TagRead;
            _ShortRFIDReader.StartReader();
        }

        private void _ShortRFIDReader_TagRead(object sender, string e)
        {
            var returnBookDTO = BooksToBeReturnedList.Where(b => b.BookCopy.BookTagShort == e).FirstOrDefault();
            if (returnBookDTO != null)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    BooksToBeReturned.MoveCurrentTo(returnBookDTO);
                    ReturnApplyPaymentCommand.Execute(returnBookDTO);
                });
            }
        }

        private void RefreshList()
        {
            BooksToBeReturnedList = new ObservableCollection<ReturnBookDTO>(_PaymentService.GetBorrowedBooks());
            BooksToBeReturned = CollectionViewSource.GetDefaultView(BooksToBeReturnedList);
        }

        private RelayCommand<ReturnBookDTO> _ReturnApplyPayment;

        /// <summary>
        /// Gets the ReturnApplyPaymentCommand.
        /// </summary>
        public RelayCommand<ReturnBookDTO> ReturnApplyPaymentCommand
        {
            get
            {
                return _ReturnApplyPayment
                    ?? (_ReturnApplyPayment = new RelayCommand<ReturnBookDTO>(
                    (r) =>
                    {
                        DialogService.ShowMessage("Do you want to continue?\n\nBook: "
                            + r.BookInfo.BookTitle + "\n"
                            + "User: " + r.LibraryUser.Fullname, "Please confirm", "Proceed", "Cancel", (x) =>
                          {
                              if (x == true)
                              {
                                  r.TransactionInfo.Execute();
                                  RefreshList();
                              }
                          });
                    },
                    (r) => true));
            }
        }


        private PaymentService _PaymentService;
        private IRFIDReader _ShortRFIDReader;

        private ICollectionView _BooksToBeReturned;
        private ObservableCollection<ReturnBookDTO> BooksToBeReturnedList;

        public ICollectionView BooksToBeReturned
        {
            get
            {
                return _BooksToBeReturned;
            }

            set
            {
                _BooksToBeReturned = value;
                RaisePropertyChanged(() => BooksToBeReturned);
            }
        }

        private IDialogService DialogService
        {
            get { return SimpleIoc.Default.GetInstance<IDialogService>(); }
        }
    }
}