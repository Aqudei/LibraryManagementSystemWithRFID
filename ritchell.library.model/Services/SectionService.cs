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

        public void AddOrUpdateSection(Section section)
        {
            using (LibUnitOfWork uow = new LibUnitOfWork())
            {
                var _section = uow.BookInfoRepository.FindById(section.Id);

                if (_section == null)
                    uow.BookInfoRepository.Add(_section);
                else
                    uow.BookInfoRepository.Update(_section);

                uow.SaveChanges();
            }
        }

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
                var sectionToDelete = uow.SectionRepository.FindById(section.Id);
                if (sectionToDelete != null)
                {
                    uow.SectionRepository.Remove(sectionToDelete);
                    uow.SaveChanges();
                }
            }
        }

        public IEnumerable<BookInfo> GetBooksUnderSection(Guid sectionId)
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

        public Section GetBookSection(BookInfo bookInfo)
        {
            using (var sectionRepo = new SectionRepository())
            {
                return sectionRepo.GetBookSection(bookInfo);
            }
        }

        public double GetChargePerDayForLateReturning(BookCopy bookCopy)
        {
            var bookCopyService = new BookCopyService();

            var bookInfo = bookCopyService.GetBookInfo(bookCopy);

            if (bookInfo == null)
                throw new InvalidOperationException("Book copy has no known book information");
            var section = GetBookSection(bookInfo);
            if (section == null)
                throw new InvalidOperationException();

            return section.LateReturningFee;
        }
    }
}
