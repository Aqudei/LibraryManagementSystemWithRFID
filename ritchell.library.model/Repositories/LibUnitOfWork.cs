using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using ritchell.library.model.Interfaces;

namespace ritchell.library.model.Repositories
{
    public class LibUnitOfWork : IUnitOfWork
    {
        private DbContext _Context;

        private IActionLogRepository _ActionLogRepository;
        private ICourseRepository _CourseRepository;
        private IDepartmentRepository _DepartmentRepository;
        private ISectionRepository _SectionRepository;
        private IBookInfoRepository _BookInfoRepository;
        private IBookCopyRepository _BookCopyRepository;
        private ILibraryUserRepository _LibraryUserRepository;
        private IHolidayRepository _HolidayRepository;
        private IBookTransactionInfoRepository _BookTransactionInfoRepository;

        public IHolidayRepository HolidayRepository
        {
            get { return _HolidayRepository = _HolidayRepository ?? new HolidayRepository(_Context); }

        }

        public IActionLogRepository ActionLogRepository
        {
            get
            {
                return _ActionLogRepository = _ActionLogRepository ?? new ActionLogRepository(_Context);
            }
        }

        public ILibraryUserRepository LibraryUserRepository
        {
            get { return _LibraryUserRepository = _LibraryUserRepository ?? new LibraryUserRepository(_Context); }
        }

        public IBookCopyRepository BookCopyRepository
        {
            get { return _BookCopyRepository = _BookCopyRepository ?? new BookCopyRepository(_Context); }
        }

        public LibUnitOfWork(DbContext context)
        {
            _Context = context;
        }

        public LibUnitOfWork()
        {
            _Context = new LibraryContext();
        }

        public ISectionRepository SectionRepository
        {
            get
            {
                _SectionRepository = _SectionRepository ?? new SectionRepository(_Context);
                return _SectionRepository;
            }
        }

        public IBookInfoRepository BookInfoRepository
        {
            get
            {
                _BookInfoRepository = _BookInfoRepository ?? new BookInfoRepository(_Context);
                return _BookInfoRepository;
            }
        }

        public IBookTransactionInfoRepository BookTransactionInfoRepository
        {
            get
            {
                return _BookTransactionInfoRepository = _BookTransactionInfoRepository ?? new BookTransactionInfoRepository(_Context);
            }
        }

        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                return _DepartmentRepository = _DepartmentRepository ?? new DepartmentRepository(_Context);
            }
        }

        public ICourseRepository CourseRepository
        {
            get
            {
                return _CourseRepository = _CourseRepository ?? new CourseRepository(_Context);
            }
        }

        public void SaveChanges()
        {
            _Context.SaveChanges();
        }

        public void Dispose()
        {
            _Context.Dispose();
        }
    }
}
