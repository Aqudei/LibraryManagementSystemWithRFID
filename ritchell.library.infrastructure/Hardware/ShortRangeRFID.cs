using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCSC;

namespace ritchell.library.infrastructure.Hardware
{
    public class ShortRangeRFID : IDisposable, IRFIDReader
    {
        private SCardContext cardContext;

        public event EventHandler<string> TagRead;

        public ShortRangeRFID()
        {
            InitCardReader();
        }

        private void InitCardReader()
        {
            cardContext = new SCardContext();
            cardContext.Establish(SCardScope.System);

            var readers = cardContext.GetReaders();
            if (readers.Length > 0)
            {
                SCardMonitor cardMonitor = new SCardMonitor(cardContext, SCardScope.System);
                cardMonitor.CardInserted += cardMonitor_CardInserted;
                
                cardMonitor.Start(readers[0]);
            }
        }

        void cardMonitor_CardInserted(object sender, CardStatusEventArgs e)
        {
            var cardReader = new SCardReader(cardContext);
            cardReader.Connect(e.ReaderName, SCardShareMode.Shared, SCardProtocol.T0 | SCardProtocol.T1);

            int rxBuffLen = 32;
            byte[] recieveBuff = new byte[rxBuffLen];
            cardReader.Transmit(new byte[] { 0xFF, 0xCA, 0x00, 0x00, 0x04 }, recieveBuff, ref rxBuffLen);

            string tag = BitConverter.ToString(recieveBuff, 0, rxBuffLen);

            var handler = TagRead;
            if (handler != null)
                handler(this, tag);

            cardReader.Disconnect(SCardReaderDisposition.Leave);
        }

        public void Dispose()
        {
            cardContext.Release();
            cardContext.Dispose();
        }
    }
}
