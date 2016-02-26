using ritchell.library.infrastructure;
using ritchell.library.infrastructure.Logging;
using ritchell.library.model.Logging;
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
        private LibraryUser _LibraryUser;

        public LibraryTransactionsAggregate(LibraryUser user) : this()
        {
            _LibraryUser = user;

            if (_LibraryUser.LibraryUserType == LibraryUser.UserType.Instructor)
            {
                CompletePayment();
            }
        }

        public LibraryTransactionsAggregate()
        {
            LibraryTransactions = new ObservableCollection<LibraryTransactionBase>();
        }

        public void CompletePayment(string adminUsername, string adminPassword)
        {
            var UserService = new LibraryUserService();

            AdminUsed = UserService.GetAuthenticatedAdmin(adminUsername, adminPassword);
            if (AdminUsed != null)
            {
                CompletePayment();
            }
            else
                throw new InvalidOperationException("Invalid admin username and password.");
        }

        private void CompletePayment()
        {
            foreach (var trans in LibraryTransactions)
            {
                var returnBookTrans = trans as ReturnBookTransaction;
                if (returnBookTrans != null)
                {
                    returnBookTrans.CompletePayment();
                    ActionLogger.Log(string.Format("{0} paid P{1:0.00} for {2} with acquisition # {3}. Admin was {4}.",
                        _LibraryUser.Fullname, RequiredFee, returnBookTrans.BookTitle, returnBookTrans.BookCopy.AcquisitionNumber, AdminUsed.Fullname));
                }
            }

            FirePropertyChanged("RequiredFee");
        }

        public void PrepareForNewUser(LibraryUser user)
        {
            _LibraryUser = user;
            Reset();
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

        public void AddTransaction(string bookTag)
        {
            if (LibraryTransactions.Where(t => t.BookTag == bookTag).Any() == false)
            {
                var trans = LibraryTransactionFactory.CreateTransaction(_LibraryUser, bookTag);
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

        public IActionLogger ActionLogger
        {
            get
            {
                return actionLogger = actionLogger ?? new DBLogger();
            }
        }

        private IActionLogger actionLogger;
        private LibraryUser AdminUsed;
    }
}
