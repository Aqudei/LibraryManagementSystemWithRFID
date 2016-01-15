using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDGenratorDebug
{
    public class RFIDShortLongDebug : ritchell.library.infrastructure.Hardware.IRFIDReader
    {
        public event EventHandler<string> TagRead;

        public void Generate()
        {
            var handler = TagRead;

            if (handler != null)
                handler(this, Guid.NewGuid().ToString());
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
