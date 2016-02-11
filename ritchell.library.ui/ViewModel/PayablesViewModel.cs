using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using ritchell.library.model;
using ritchell.library.model.LibraryTransactions;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ritchell.library.ui.ViewModel
{
    public class PayablesViewModel : ViewModelBase
    {
        private PaymentService _PaymentService;
        private ObservableCollection<Payable> _Payables;

        private ICollectionView _PayablesCollectionView;

        public ICollectionView PayablesCollectionView
        {
            get { return _PayablesCollectionView; }
            set
            {
                _PayablesCollectionView = value;
                RaisePropertyChanged(() => PayablesCollectionView);
            }
        }

        public PayablesViewModel(PaymentService paymentService)
        {
            if (IsInDesignMode == false)
            {
                _PaymentService = paymentService;
                RefreshPayables();
            }
            else
            {
                Payables = new ObservableCollection<Payable>();
                Payables.Add(new Payable
                {
                    AmountToPay = 250,
                    BookInvolved = "Rizal's Life",
                    TransactionInfo = new TransactionInfo
                    {
                        IsPaid = false,
                    },
                    UserInvolved = "Aqudei"
                });
            }
        }

        private void RefreshPayables()
        {
            Payables = new ObservableCollection<Payable>(_PaymentService.GetReturnedBooksPayables());
            PayablesCollectionView = CollectionViewSource.GetDefaultView(Payables);
        }

        private RelayCommand<Payable> _CompletePaymentCommand;

        /// <summary>
        /// Gets the CompletePayment.
        /// </summary>
        public RelayCommand<Payable> CompletePaymentCommand
        {
            get
            {
                return _CompletePaymentCommand
                    ?? (_CompletePaymentCommand = new RelayCommand<Payable>(
                    (transInfo) =>
                    {
                        DialogService.ShowMessage("Do you want to continue?\n\nBook: "
                         + transInfo.BookInvolved + "\n"
                         + "User: " + transInfo.UserInvolved, "Please confirm", "Proceed", "Cancel", (x) =>
                         {
                             if (x == true)
                             {
                                 _PaymentService.CompletePayment(transInfo);
                                 RefreshPayables();
                             }
                         });
                    },
                    (ti) => true));
            }
        }

        private RelayCommand _RefreshPayablesCommand;

        /// <summary>
        /// Gets the RefreshPayablesCommand.
        /// </summary>
        public RelayCommand RefreshPayablesCommand
        {
            get
            {
                return _RefreshPayablesCommand
                    ?? (_RefreshPayablesCommand = new RelayCommand(
                    () =>
                    {
                        RefreshPayables();
                    },
                    () => true));
            }
        }

        public ObservableCollection<Payable> Payables
        {
            get
            {
                return _Payables;
            }

            set
            {
                _Payables = value;
                RaisePropertyChanged(() => Payables);
            }
        }

        private IDialogService DialogService
        {
            get { return SimpleIoc.Default.GetInstance<IDialogService>(); }
        }
    }
}
