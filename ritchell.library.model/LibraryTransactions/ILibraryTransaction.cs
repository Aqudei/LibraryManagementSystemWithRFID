using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.LibraryTransactions
{
    public interface ILibraryTransaction
    {
        void Execute();
        DateTime TransactionDate { get; set; }
    }
}
