using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Hardware
{
    public class NullRFID : RFIDReaderBase, IRFIDReader
    {
        public NullRFID()
        { }

        public override void Dispose()
        {
            Debug.WriteLine("NullRFID Disposed!");
        }
    }
}
