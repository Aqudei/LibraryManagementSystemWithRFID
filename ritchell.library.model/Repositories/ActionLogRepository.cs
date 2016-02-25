using ritchell.library.infrastructure;
using ritchell.library.model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ritchell.library.model.Repositories
{
    public class ActionLogRepository : RepositoryBase<infrastructure.Logging.ActionLog>, IActionLogRepository
    {
        public ActionLogRepository() : this(new LibraryContext())
        {

        }

        public ActionLogRepository(DbContext context)
            : base(context)
        {
        }

        public void Dispose()
        {
            _Context.Dispose();
        }
    }
}
