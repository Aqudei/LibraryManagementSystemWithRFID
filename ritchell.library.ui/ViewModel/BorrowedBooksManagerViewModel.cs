using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ritchell.library.model;
using ritchell.library.model.LibraryTransactions;
using System.Collections.ObjectModel;
using System;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;

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
        }

        private void RefreshList()
        {
            BooksToBeReturned = new ObservableCollection<ReturnBookDTO>(_PaymentService.GetBorrowedBooks());
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

        private ObservableCollection<ReturnBookDTO> _BooksToBeReturned;
        private PaymentService _PaymentService;

        public ObservableCollection<ReturnBookDTO> BooksToBeReturned
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