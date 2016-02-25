using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Logging
{
    public interface IActionLogger
    {
        void Log(string action);
        IEnumerable<ActionLog> GetActionLogs(DateTime datetime);
        IEnumerable<ActionLog> GetActionLogs();
        IEnumerable<ActionLog> GetActionLogs(DateTime datetime1, DateTime datetime2);
    }
}
