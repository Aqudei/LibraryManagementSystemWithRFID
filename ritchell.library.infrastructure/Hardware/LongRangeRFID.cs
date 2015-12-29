using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Hardware
{
    public class LongRangeRFID : IRFIDReader
    {
        public event EventHandler<string> TagRead;

        public void Dispose()
        {
            
        }
    }
}
