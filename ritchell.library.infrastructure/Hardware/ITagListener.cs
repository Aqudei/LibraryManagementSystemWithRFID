using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Hardware
{
    public interface ITagListener
    {
        void TagRead(RFIDTag tag);
    }
}
