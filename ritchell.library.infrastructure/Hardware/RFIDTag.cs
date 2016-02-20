using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Hardware
{
    public class RFIDTag
    {
        public enum TagType
        { Short, Long}

        public TagType RFIDTagType { get; set; }
        public string Tag { get; set; }
    }
}
