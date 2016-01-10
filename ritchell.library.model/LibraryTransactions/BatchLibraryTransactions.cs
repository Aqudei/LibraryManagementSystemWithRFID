using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class BatchLibraryTransactions
    {
        private ObservableCollection<ILibraryTransaction> _LibraryTransactions;
        private readonly LibraryUser _User;

        public BatchLibraryTransactions(LibraryUser user)
        {
            _User = user;
            LibraryTransactions = new ObservableCollection<ILibraryTransaction>();
        }

        public ObservableCollection<ILibraryTransaction> LibraryTransactions
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

        public void RemoveTransaction(ILibraryTransaction transaction)
        {
            LibraryTransactions.Remove(transaction);
        }

        public void Reset()
        {
            LibraryTransactions.Clear();
        }
    }
}
