using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure.Logging;
using ritchell.library.model.Interfaces;
using ritchell.library.model.Repositories;

namespace ritchell.library.model.Logging
{
    public class DBLogger : infrastructure.Logging.ActionLoggerBase, infrastructure.Logging.IActionLogger
    {

        public override IEnumerable<ActionLog> GetActionLogs(DateTime datetime)
        {
            using (var actionLogRepo = new ActionLogRepository())
            {
                var _datetime = datetime.Date;
                return actionLogRepo.Where(a => a.LogDate == _datetime).ToList();
            }
        }

        public override IEnumerable<ActionLog> GetActionLogs(DateTime datetime1, DateTime datetime2)
        {
            using (var actionLogRepo = new ActionLogRepository())
            {
                var _datetime1 = datetime1.Date;
                var _datetime2 = datetime2.Date;

                return actionLogRepo.Where(a => a.LogDate >= _datetime1 && a.LogDate <= _datetime2).ToList();
            }
        }

        public override void Log(DateTime logDateTime, string action)
        {
            using (var uow = new LibUnitOfWork())
            {
                var newLog = new ActionLog();
                newLog.Action = action;
                newLog.LogDate = logDateTime.Date;
                newLog.LogTime = logDateTime;
                uow.ActionLogRepository.Add(newLog);
                uow.SaveChanges();
            }
        }
    }
}
