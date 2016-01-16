using ritchell.library.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.ui.client.ViewModels.VMMessages
{
    public class UserEvent : EventArgs
    {
        public enum UserEventType
        {
            Login, Logout
        }

        public UserEventType LibraryUserEventType { get; set; }

        public LibraryUser LibraryUser { get; set; }
    }
}
