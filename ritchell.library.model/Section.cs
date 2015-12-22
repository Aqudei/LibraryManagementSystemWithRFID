using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;

namespace ritchell.library.model
{
    public class Section : EntityBase<Guid>
    {

        public string Name { get; set; }
        public double LateReturningFee { get; set; }
        public int MaxDaysAllowedForBorrowing { get; set; }

        public ICollection<BookInfo> BookInfos { get; set; }

        public Section()
        {
            Id = Guid.NewGuid();
            BookInfos = new List<BookInfo>();
        }
    }
}
