using ritchell.library.infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model
{
    public class Course : EntityBase<Guid>
    {
        public Guid DepartmentId { get; set; }
        public string CourseName { get; set; }
        
        public Course()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return CourseName;
        }
    }
}
