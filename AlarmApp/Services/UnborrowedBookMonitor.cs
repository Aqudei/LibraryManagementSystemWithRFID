using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ritchell.library.model.Services;
using ritchell.library.infrastructure.Hardware;

namespace AlarmApp.Services
{
    public class UnborrowedBookMonitor
    {
        public event EventHandler<Models.BookCopyWithInfo> UnborrowedIsGoingOut;

        private DispatcherTimer monitorTimer;
        private BookCopyService _BookCopyService;

        public UnborrowedBookMonitor(BookCopyService bookCopyService,
            IRFIDReader longRangeReader)
        {
            _BookCopyService = bookCopyService;
            longRangeReader.TagRead += LongRangeReader_TagRead;
        }

        private void LongRangeReader_TagRead(object sender, string e)
        {
            var bookCopy = _BookCopyService.FindByLongRange(e);
            if (bookCopy != null && bookCopy.IsBorrowed == false)
            {
                var handler = UnborrowedIsGoingOut;
                if (handler != null)
                {
                    handler(this, new Models.BookCopyWithInfo
                    {
                        BookCopy = bookCopy,
                        BookInfo = _BookCopyService.GetBookInfo(bookCopy)
                    });
                }
            }
        }
    }
}
