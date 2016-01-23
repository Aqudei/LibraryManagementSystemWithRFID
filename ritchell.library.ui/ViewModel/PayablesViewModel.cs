using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.model;
using ritchell.library.model.LibraryTransactions;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.ui.ViewModel
{
    public class PayablesViewModel : ViewModelBase
    {
        private PaymentService _PaymentService;
        private ObservableCollection<Payable> _Payables;

        public PayablesViewModel(PaymentService paymentService)
        {
            _PaymentService = paymentService;
            RefreshPayables();
        }

        private void RefreshPayables()
        {
            Payables = new ObservableCollection<Payable>(_PaymentService.GetPayableTransactions());
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
                    (ti) =>
                    {
                        _PaymentService.CompletePayment(ti);
                        RefreshPayables();
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
    }
}
