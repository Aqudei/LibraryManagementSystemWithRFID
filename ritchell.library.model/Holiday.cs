using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;

namespace ritchell.library.model
{
    public class Holiday : EntityBase<Guid>
    {
        public Holiday()
        {
            Id = Guid.NewGuid();
        }

        public DateTime Day { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
