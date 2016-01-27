using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Hardware
{
    public class NullRFID : IRFIDReader
    {
        public event EventHandler<string> TagRead;

        public void Dispose()
        {
            Debug.WriteLine("Null RFID disposed");
        }

        public void StartReader()
        {
            Debug.WriteLine("Null RFID started");
        }

        public void StopReader()
        {
            Debug.WriteLine("Null RFID stopped");
        }
    }
}
