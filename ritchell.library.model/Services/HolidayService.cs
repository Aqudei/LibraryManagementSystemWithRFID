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

        public void MarkSatSunAsHolidays()
        {
            using (var uow = new LibUnitOfWork())
            {
                var startDay = DateTime.Now.Date;

                while (startDay != new DateTime(startDay.Year, 12, 31))
                {
                    if (startDay.DayOfWeek == DayOfWeek.Sunday || startDay.DayOfWeek == DayOfWeek.Saturday)
                    {
                        //todo : add this day to database
                        var newHoliday = new Holiday();
                        newHoliday.Day = startDay;
                        newHoliday.Description = "Weekends";
                        uow.HolidayRepository.AddOrUpdate(newHoliday);
                    }
                    startDay = startDay.AddDays(1);
                }
                uow.SaveChanges();
            }
        }

        public IEnumerable<Holiday> GetHolidays()
        {
            using (var holidayRepo = new HolidayRepository())
            {
                return holidayRepo.GetAll();
            }
        }

        public void AddHoliday(Holiday newHoliday)
        {
            using (var uow = new LibUnitOfWork())
            {
                uow.HolidayRepository.AddOrUpdate(newHoliday);
                uow.SaveChanges();
            }
        }

        public void DeleteHoliday(Holiday holiday)
        {
            using (var uow = new LibUnitOfWork())
            {
                var h = uow.HolidayRepository.FindById(holiday.Id);
                uow.HolidayRepository.Remove(h);
                uow.SaveChanges();
            }
        }
    }
}
