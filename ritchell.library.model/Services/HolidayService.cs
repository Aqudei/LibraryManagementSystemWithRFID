using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.model.Repositories;

namespace ritchell.library.model.Services
{
    public class HolidayService
    {
        public DateTime GetNonHolidayDateAfter(DateTime startDate)
        {
            var repo = new HolidayRepository();

            for (DateTime i = startDate; ; i = i.AddDays(1))
            {
                if (repo.IsHoliday(startDate))
                    continue;

                return startDate;
            }

        }
    }
}
