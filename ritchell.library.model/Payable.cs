using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.model.LibraryTransactions;

namespace ritchell.library.model
{
    //Reponsible  for payment of book later. Includes fields that are use for display.
    public class Payable
    {
        public double AmountToPay { get; set; }
        public string BookInvolved { get; set; }
        public Guid LibraryUserId { get; set; }
        public TransactionInfo TransactionInfo { get; set; }
        public string UserInvolved { get; set; }
    }
}
