using ritchell.library.model.LibraryTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model
{
    public class PaymentsTransaction
    {
        private TransactionInfo _TransactionInfo;

        public PaymentsTransaction(TransactionInfo transInfo)
        {
            _TransactionInfo = transInfo;
        }
    }
}
