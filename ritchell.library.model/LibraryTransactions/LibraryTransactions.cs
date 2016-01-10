using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public class LibraryTransactions 
    {
        List<ILibraryTransaction> _LibraryTransactions;
        private readonly LibraryUser _User;

        private LibraryTransactions(LibraryUser user)
        {
            _User = user;
            _LibraryTransactions = new List<ILibraryTransaction>();
        }

        public void AddTransaction(ILibraryTransaction transaction)
        {
            _LibraryTransactions.Add(transaction);
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
