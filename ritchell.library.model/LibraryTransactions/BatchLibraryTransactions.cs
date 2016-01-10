using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class BatchLibraryTransactions
    {
        List<ILibraryTransaction> _LibraryTransactions;
        private readonly LibraryUser _User;

        public BatchLibraryTransactions(LibraryUser user)
        {
            _User = user;
            _LibraryTransactions = new List<ILibraryTransaction>();
        }

        public void AddTransaction(string bookTag)
        {
            if (_LibraryTransactions.Where(t => t.BookTag == bookTag).Any() == false)
                LibraryTransactionFactory.CreateTransaction(_User.Id, bookTag);
        }


        public void ExecuteAll()
        {
            foreach (var trans in _LibraryTransactions)
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
            _LibraryTransactions.Remove(transaction);
        }

        public void Reset(ILibraryTransaction transaction)
        {
            _LibraryTransactions.Clear();
        }
    }
}
