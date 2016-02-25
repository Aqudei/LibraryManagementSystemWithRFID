using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Logging
{
    public class ActionLog : EntityBase<Guid>
    {
        public ActionLog()
        {
            Id = Guid.NewGuid();
        }
        public DateTime LogDate { get; set; }
        public DateTime LogTime { get; set; }
        public string Action { get; set; }
    }
}
