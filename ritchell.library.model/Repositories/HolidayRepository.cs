﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using ritchell.library.model.Interfaces;

namespace ritchell.library.model.Repositories
{
    public class HolidayRepository : RepositoryBase<Holiday>, IHolidayRepository
    {
        public HolidayRepository() : this(new LibraryContext())
        { }

        public HolidayRepository(DbContext context)
            : base(context)
        { }

        public bool IsHoliday(DateTime date)
        {
            return _Context.Set<Holiday>().ToList().Where(h => h.Day.Date.Equals(date.Date)).Any();
        }


        public void AddOrUpdate(Holiday holiday)
        {
            var old = _Context.Set<Holiday>().ToList().Where(h => h.Day.Date.Equals(holiday.Day.Date)).FirstOrDefault();

            if (old != null)
            {
                var entoty = _Context.Entry(old).Entity;
                entoty.Day = holiday.Day;
                entoty.Description = holiday.Description;
            }
            else
            {
                _Context.Set<Holiday>().Add(holiday);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _Context.Dispose();
                    _Context = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~HolidayRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

    }
}
