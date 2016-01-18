using ritchell.library.infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model
{
    public class Department : EntityBase<Guid>
    {
        public Department()
        {
            Id = Guid.NewGuid();
        }
        public string LongName { get; set; }
        public string ShortName { get; set; }
    }
}