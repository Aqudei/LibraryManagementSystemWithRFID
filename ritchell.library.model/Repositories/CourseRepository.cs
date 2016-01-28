using ritchell.library.infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ritchell.library.model.Interfaces;

namespace ritchell.library.model.Repositories
{
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(DbContext context) : base(context)
        { }

        public CourseRepository() : this(new LibraryContext())
        {}

        public void Dispose()
        {
            _Context.Dispose();
        }

        public IEnumerable<Course> GetCourseByDepartmentId(Guid DepartmentId)
        {
            return _Context.Set<Course>().Where(c => c.DepartmentId.Equals(DepartmentId)).ToList();
        }
    }
}
