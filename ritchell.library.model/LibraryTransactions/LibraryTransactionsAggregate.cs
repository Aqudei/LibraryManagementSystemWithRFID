using ritchell.library.infrastructure;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class LibraryTransactionsAggregate : INPCBase
    {
        private ObservableCollection<LibraryTransactionBase> _LibraryTransactions;
        private readonly LibraryUser _LibraryUser;
        private bool _PaymentPaid;

        public LibraryTransactionsAggregate(LibraryUser user)
        {
            LibraryTransactions = new ObservableCollection<LibraryTransactionBase>();
            _LibraryUser = user;

            if (_LibraryUser.LibraryUserType == LibraryUser.UserType.Teacher)
            {
                PayNecessaryFees();
            }
        }

        private void PayNecessaryFees()
        {
            foreach (var trans in LibraryTransactions)
            {
                var returnBookTrans = trans as ReturnBookTransaction;
                if (returnBookTrans != null)
                {
                    returnBookTrans.CompletePayment();
                }
            }

            FirePropertyChanged("RequiredFee");
        }

        public void PayNecessaryFees(string adminUsername, string adminPassword)
        {
            var UserService = new LibraryUserService();

            var admin = UserService.GetAuthenticatedAdmin(adminUsername, adminPassword);
            if (admin != null)
                PayNecessaryFees();
        }

        public ObservableCollection<LibraryTransactionBase> LibraryTransactions
        {
            get
            {
                return _LibraryTransactions;
            }
            set
            {
                _LibraryTransactions = value;
            }
        }

        public bool PaymentPaid
        {
            get { return _PaymentPaid; }
            set
            {
                _PaymentPaid = value;
                FirePropertyChanged("PaymentPaid");
            }
        }


        public void AddTransaction(string bookTag)
        {
            if (LibraryTransactions.Where(t => t.BookTag == bookTag).Any() == false)
            {
                var trans = LibraryTransactionFactory.CreateTransaction(_LibraryUser.Id, bookTag);
                _LibraryTransactions.Add(trans);
            }
        }

        public void ExecuteAll()
        {
            foreach (var trans in LibraryTransactions)
            {
                try
                {
                    trans.Execute();
                }
                catch (Exception)
                { }
            }
        }

        public void RemoveTransaction(LibraryTransactionBase transaction)
        {
            LibraryTransactions.Remove(transaction);
        }

        public void Reset()
        {
            LibraryTransactions.Clear();
        }

        public double RequiredFee
        {
            get
            {
                double totalFee = 0;
                foreach (var trans in LibraryTransactions)
                {
                    var returnBookTrans = trans as ReturnBookTransaction;
                    if (returnBookTrans != null)
                    {
                        totalFee += returnBookTrans.RequiredFee;
                    }
                }

                return totalFee;
            }
        }

    }
}
