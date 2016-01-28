using ritchell.library.model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.Services
{
    public class CourseService
    {
        public IEnumerable<Course> GetAllCourses()
        {
            using (var courseRepo = new CourseRepository())
            {
                return courseRepo.GetAll();
            }
        }

        public void AddOrUpdate(Course c)
        {
            using (LibUnitOfWork uow = new LibUnitOfWork())
            {
                var course = uow.BookInfoRepository.FindById(c.Id);

                if (course == null)
                    uow.CourseRepository.Add(c);
                else
                    uow.CourseRepository.Update(c);

                uow.SaveChanges();
            }
        }

        public void DeleteCourse(Course course)
        {
            using (LibUnitOfWork uow = new LibUnitOfWork())
            {
                var _course = uow.CourseRepository.FindById(course.Id);
                uow.CourseRepository.Remove(_course);
                uow.SaveChanges();
            }
        }
    }
}
