using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.model;
using ritchell.library.model.LibraryTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.ui.client.ViewModels
{
    public class PayablesViewModel : ViewModelBase
    {
        public PayablesViewModel(IEnumerable<Payable> payables)
        {
            Payables = payables;
        }

        private IEnumerable<Payable> payables;

        public IEnumerable<Payable> Payables
        {
            get
            {
                return payables;
            }

            set
            {
                payables = value;
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
