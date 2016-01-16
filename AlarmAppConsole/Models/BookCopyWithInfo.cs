using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.model;

namespace AlarmApp.Models
{
    public class BookCopyWithInfo
    {
        public BookInfo  BookInfo { get; set; }
        public BookCopy BookCopy { get; set; }
    }
}
