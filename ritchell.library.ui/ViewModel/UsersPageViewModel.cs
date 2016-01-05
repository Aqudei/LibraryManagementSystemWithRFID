using System;
using GalaSoft.MvvmLight;
using ritchell.library.model;

namespace ritchell.library.ui.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class UsersPageViewModel : WithEditableItems<LibraryUser>
    {
        /// <summary>
        /// Initializes a new instance of the UsersPageViewModel class.
        /// </summary>
        public UsersPageViewModel()
        {
        }

        public override void DeleteItemCommandHandler()
        {
            throw new NotImplementedException();
        }

        protected override void NewItemCommandHandler()
        {
            throw new System.NotImplementedException();
        }

        protected override void SaveItemCommandHandler()
        {
            throw new System.NotImplementedException();
        }
    }
}