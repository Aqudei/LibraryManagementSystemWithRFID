using ritchell.library.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.ui.client.ViewModels.VMMessages
{
    public class UserHasLogin
    {
        LibraryUser _CurrentLibraryUser;

        public LibraryUser CurrentLibraryUser
        {
            get
            {
                return _CurrentLibraryUser;
            }

            set
            {
                _CurrentLibraryUser = value;
            }
        }
    }
}
