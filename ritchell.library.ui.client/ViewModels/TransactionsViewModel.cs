using GalaSoft.MvvmLight;
using ritchell.library.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.ui.client.ViewModels
{
    public class TransactionsViewModel : ViewModelBase
    {
        private LibraryUser _LibraryUser;

        public TransactionsViewModel(LibraryUser libUser)
        {
            _LibraryUser = libUser;
        }
    }
}
