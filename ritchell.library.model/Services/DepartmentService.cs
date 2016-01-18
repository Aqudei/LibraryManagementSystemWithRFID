using ritchell.library.model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.Services
{
    public class DepartmentService
    {
        public IEnumerable<Department> GetDepartments()
        {
            using (var deptRepo = new DepartmentRepository())
            {
                return deptRepo.GetAll();
            }
        }

        public void DeleteDepartment(Department dept)
        {
            using (var uow = new LibUnitOfWork())
            {
                var _dept = uow.DepartmentRepository.FindById(dept.Id);
                if (_dept != null)
                {
                    uow.DepartmentRepository.Remove(_dept);
                    uow.SaveChanges();
                }
            }
        }

        public void AddOrUpdate(Department dept)
        {
            using (LibUnitOfWork uow = new LibUnitOfWork())
            {
                var _Dept = uow.DepartmentRepository.FindById(dept.Id);

                if (_Dept == null)
                    uow.DepartmentRepository.Add(dept);
                else
                    uow.DepartmentRepository.Update(dept);

                uow.SaveChanges();
            }
        }
    }
}
