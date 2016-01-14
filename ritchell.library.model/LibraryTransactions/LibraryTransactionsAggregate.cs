using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class LibraryTransactionsAggregate
    {
        private ObservableCollection<LibraryTransactionBase> _LibraryTransactions;
        private readonly LibraryUser _User;

        public LibraryTransactionsAggregate PayNecessaryFees()
        {
            return this;
        }

        public LibraryTransactionsAggregate(LibraryUser user)
        {
            _User = user;
            LibraryTransactions = new ObservableCollection<LibraryTransactionBase>();
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
                LibraryTransactionFactory.CreateTransaction(_User.Id, bookTag);
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

        public double ComputeNecessaryFee()
        {
            double necessaryFees = 0;
            foreach (var trans in LibraryTransactions)
            {
                var borrowTrans = trans as ReturnBookTransaction;
                if (borrowTrans != null)
                {
                    
                }
            }

            return necessaryFees;
        }
    }
}
