using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.model;
using ritchell.library.model.LibraryTransactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ritchell.library.ui.client.ViewModels
{
    public class PayablesViewModel : ViewModelBase
    {

        public PayablesViewModel(LibraryUser libraryUser)
        {
            Payables = new ObservableCollection<Payable>(PaymentService.GetReturnedBooksPayables(libraryUser));
        }

        private ObservableCollection<Payable> payables;

        public ObservableCollection<Payable> Payables
        {
            get
            {
                return payables;
            }

            set
            {
                payables = value;
                RaisePropertyChanged(() => Payables);
            }
        }

        private RelayCommand<Payable> _CompletePayment;

        /// <summary>
        /// Gets the CompletePayment.
        /// </summary>
        public RelayCommand<Payable> CompletePayment
        {
            get
            {
                return _CompletePayment
                    ?? (_CompletePayment = new RelayCommand<Payable>(
                    (p) =>
                    {

                    },
                    (p) => true));
            }
        }

        public PaymentService PaymentService
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PaymentService>();
            }
        }
    }
}
