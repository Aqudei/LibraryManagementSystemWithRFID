using GalaSoft.MvvmLight;
using ritchell.library.model;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ritchell.library.ui.ViewModel
{
    public class DepartmentsViewModel : WithEditableItems<Department>
    {
        private DepartmentService _DepartmentService;

        public DepartmentsViewModel(DepartmentService departmentService)
        {
            _DepartmentService = departmentService;

            items = new System.Collections.ObjectModel.ObservableCollection<Department>(_DepartmentService.GetDepartments());
            ItemsCollectionView = CollectionViewSource.GetDefaultView(items);
        }

        public override void DeleteItemCommandHandler()
        {
            var currentDept = ItemsCollectionView.CurrentItem as Department;
            if (currentDept != null)
            {
                _DepartmentService.DeleteDepartment(currentDept);
                items.Remove(currentDept);
            }
        }

        public override void EditItemCommandHandler()
        { }

        protected override void NewItemCommandHandler()
        {
            var newDept = new Department();
            items.Add(newDept);
            ItemsCollectionView.MoveCurrentTo(newDept);
        }

        protected override void SaveItemCommandHandler()
        {
            var currentDept = ItemsCollectionView.CurrentItem as Department;
            if (currentDept != null)
            {
                _DepartmentService.AddOrUpdate(currentDept);
            }
        }
    }
}
