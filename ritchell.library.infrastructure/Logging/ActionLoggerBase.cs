using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Logging
{
    public abstract class ActionLoggerBase : IActionLogger
    {
        public IEnumerable<ActionLog> GetActionLogs()
        {
            throw new NotImplementedException();
        }

        public abstract IEnumerable<ActionLog> GetActionLogs(DateTime datetime);


        public abstract IEnumerable<ActionLog> GetActionLogs(DateTime datetime1, DateTime datetime2);

        public void Log(string action)
        {
            Log(DateTime.Now, action);
        }

        public abstract void Log(DateTime now, string action);
    }
}
