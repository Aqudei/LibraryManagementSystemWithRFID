using System;
using GalaSoft.MvvmLight;
using ritchell.library.model;
using ritchell.library.model.Services;
using ritchell.library.model.Repositories;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;

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
        private readonly LibraryUserService _LibraryUserService;
        private ICollectionView _DepartmentSource;
        private string _PasswordCopy;
        private DepartmentService _DepartmentService;
        private CourseService _CourseService;

        /// <summary>
        /// Initializes a new instance of the UsersPageViewModel class.
        /// </summary>
        public UsersPageViewModel(LibraryUserService libraryUserService,
            DepartmentService departmentService, CourseService courseService)
        {
            _LibraryUserService = libraryUserService;
            _DepartmentService = departmentService;
            _CourseService = courseService;

            using (var repo = new LibraryUserRepository())
            {
                items = new ObservableCollection<LibraryUser>(repo.GetAll());
                ItemsCollectionView = CollectionViewSource.GetDefaultView(items);
                ItemsCollectionView.SortDescriptions.Add(new SortDescription("Fullname", ListSortDirection.Ascending));
            }

            DepartmentSource = CollectionViewSource.GetDefaultView(new ObservableCollection<Department>(_DepartmentService.GetDepartments()));
            DepartmentSource.CurrentChanged += DepartmentSource_CurrentChanged;
            var courses = new ObservableCollection<Course>(courseService.GetAllCourses());
            CoursesViewSource = CollectionViewSource.GetDefaultView(courses);
        }

        private void DepartmentSource_CurrentChanged(object sender, EventArgs e)
        {
            var selectedDepartment = (DepartmentSource.CurrentItem as Department);

            CoursesViewSource.Filter = new Predicate<object>((x) =>
            {
                if (selectedDepartment == null)
                    return false;

                return (x as Course).DepartmentId.Equals(selectedDepartment.Id);
            });
            CoursesViewSource.Refresh();
        }

        public override void DeleteItemCommandHandler()
        {
            var currentUser = ItemsCollectionView.CurrentItem as LibraryUser;
            if (currentUser != null)
            {
                _LibraryUserService.DeleteUser(currentUser);
                items.Remove(currentUser);
            }
        }

        protected override void NewItemCommandHandler()
        {
            LibraryUser user = new LibraryUser();
            items.Add(user);
            ItemsCollectionView.MoveCurrentTo(user);
            ItemsCollectionView.Refresh();
        }

        public override bool InputFieldsAreValid()
        {
            var current = ItemsCollectionView.CurrentItem as LibraryUser;
            if (current == null)
                return false;

            if (current.DepartmentId == null)
                return false;

            if (string.IsNullOrEmpty(current.Password))
                return false;

            if (string.IsNullOrEmpty(PasswordCopy))
                return false;

            return current.Password == PasswordCopy;
        }


        public string PasswordCopy
        {
            get { return _PasswordCopy; }
            set
            {
                _PasswordCopy = value;
                RaisePropertyChanged(() => PasswordCopy);
                SaveItemCommand.RaiseCanExecuteChanged();
            }
        }

        protected override void SaveItemCommandHandler()
        {
            var currentUser = ItemsCollectionView.CurrentItem as LibraryUser;
            _LibraryUserService.AddOrUpdateLibraryUser(currentUser);
            ItemsCollectionView.Refresh();
            PasswordCopy = "";
        }



        public override void EditItemCommandHandler()
        {

        }

        public Array UserTypes
        {
            get
            {
                return Enum.GetValues(typeof(LibraryUser.UserType));
            }
        }

        public void UpdateEnabledDisabled()
        {
            RaisePropertyChanged(() => CourseApplicable);
        }

        public ICollectionView DepartmentSource
        {
            get
            {
                return _DepartmentSource;
            }
            set
            {
                _DepartmentSource = value;
                RaisePropertyChanged(() => DepartmentSource);
            }
        }

        public ICollectionView CoursesViewSource { get; private set; }

        public bool CourseApplicable
        {
            get
            {
                var currentUser = ItemsCollectionView.CurrentItem as LibraryUser;

                return (currentUser != null) && (currentUser.LibraryUserType == LibraryUser.UserType.Student);
            }
        }
    }
}