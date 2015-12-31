using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.model.Interfaces;
using ritchell.library.model.Repositories;

namespace ritchell.library.model.Services
{
    public class SectionService
    {
        private readonly HolidayService holidayService;

        public SectionService()
        {
            holidayService = new HolidayService();
        }

        public void CreateNewSection(Section section)
        {
            using (var uow = new LibUnitOfWork())
            {
                uow.SectionRepository.Add(section);
                uow.SaveChanges();
            }
        }

        public IEnumerable<Section> GetAllSections()
        {
            using (var uow = new LibUnitOfWork())
            {
                return uow.SectionRepository.GetAll();
            }
        }

        public void SaveSection(Section section)
        {
            using (var uow = new LibUnitOfWork())
            {
                var sect = uow.SectionRepository.FindById(section.Id);

                if (sect == null)
                    uow.SectionRepository.Add(section);
                else
                    uow.SectionRepository.Update(section);

                uow.SaveChanges();
            }
        }

        public void DeleteSection(Section section)
        {
            using (var uow = new LibUnitOfWork())
            {
                uow.SectionRepository.Remove(section);
                uow.SaveChanges();
            }
        }

        public IEnumerable<BookInfo> GetBooks(Guid sectionId)
        {
            using (var uow = new LibUnitOfWork())
            {
                return uow.SectionRepository.GetBooks(sectionId);
            }
        }

        public DateTime GetBookReturnDateFromNow(BookInfo bookInfo)
        {
            using (var sectionRepo = new SectionRepository())
            {
                var section = sectionRepo.FindById(bookInfo.SectionId);
                if (section == null)
                    throw new InvalidOperationException("I don't know which section does "
                        + bookInfo.BookTitle + " belongs to.");

                return holidayService.GetNonHolidayDateAfter(DateTime.Now.AddDays(section.MaxDaysAllowedForBorrowing));
            }
        }
    }
}
