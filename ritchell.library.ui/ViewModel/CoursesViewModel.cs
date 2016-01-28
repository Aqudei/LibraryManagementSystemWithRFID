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
    public class CoursesViewModel : WithEditableItems<Course>
    {
        /// <summary>
        /// Initializes a new instance of the CoursesViewModel class.
        /// </summary>
        public CoursesViewModel()
        {
        }

        public override void DeleteItemCommandHandler()
        {
            throw new NotImplementedException();
        }

        public override void EditItemCommandHandler()
        {
            throw new NotImplementedException();
        }

        protected override void NewItemCommandHandler()
        {
            throw new NotImplementedException();
        }

        protected override void SaveItemCommandHandler()
        {
            throw new NotImplementedException();
        }
    }
}