using System;
using GalaSoft.MvvmLight;
using ritchell.library.model;
using System.Collections.ObjectModel;
using ritchell.library.model.Services;
using System.Windows.Data;

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
        private CourseService _CourseService;
        private DepartmentService _DepartmentService;

        /// <summary>
        /// Initializes a new instance of the CoursesViewModel class.
        /// </summary>
        public CoursesViewModel(DepartmentService departmentService,
            CourseService courseService)
        {
            _CourseService = courseService;
            _DepartmentService = departmentService;

            Departments = new ObservableCollection<Department>(departmentService.GetDepartments());
            items = new ObservableCollection<Course>(courseService.GetAllCourses());
            ItemsCollectionView = CollectionViewSource.GetDefaultView(items);
        }

        public ObservableCollection<Department> Departments { get; set; }

        public override void DeleteItemCommandHandler()
        {
            var cuurentCourse = ItemsCollectionView.CurrentItem as Course;
            if (cuurentCourse != null)
            {
                _CourseService.DeleteCourse(cuurentCourse);
                items.Remove(cuurentCourse);
            }
        }

        public override void EditItemCommandHandler()
        {}

        protected override void NewItemCommandHandler()
        {
            var newCourse = new Course();
            items.Add(newCourse);
            ItemsCollectionView.MoveCurrentTo(newCourse);
        }

        protected override void SaveItemCommandHandler()
        {
            var currentCourse = ItemsCollectionView.CurrentItem as Course;
            if (currentCourse != null)
            {
                _CourseService.AddOrUpdate(currentCourse);
            }
        }
    }
}