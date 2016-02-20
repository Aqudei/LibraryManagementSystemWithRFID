using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Hardware
{
    public class FakeRFID : RFIDReaderBase, IRFIDReader
    {
        private BackgroundWorker bgWorker;

        public static Guid[] FakeGuids = new Guid[3];

        public FakeRFID()
        {
            FakeGuids[0] = Guid.Parse("{F7561AF5-0C6D-4A8A-B925-2D6AB2B673A9}");
            FakeGuids[1] = Guid.Parse("{B5200174-814F-432B-A1F7-7FF5CC2D90D5}");
            FakeGuids[2] = Guid.Parse("{FD8476BA-5EEC-4FDF-AF7F-D50778E77D59}");
            FakeGuids[3] = Guid.Parse("{C1E2F573-DB67-4A8D-B156-5A353E202374}");
            FakeGuids[4] = Guid.Parse("{142000F8-FD5A-4870-965C-4F7287F4FA01}");
            FakeGuids[5] = Guid.Parse("{2D8B8E84-E25A-4BD3-9904-BD90115CC97E}");
            FakeGuids[6] = Guid.Parse("{48F9AC5B-8CE2-4B57-89EE-EC6F284A0134}");

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.WorkerSupportsCancellation = true;
        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            while (bgWorker.CancellationPending == false)
            {
                Thread.Sleep(2000);
                bgWorker.ReportProgress(0, FakeGuids[r.Next(2)].ToString());
            }
        }

        private void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (IsMonitoring == true)
            {
                foreach (var listener in TagListeners)
                {
                    listener.TagRead(new RFIDTag
                    {
                        RFIDTagType = RFIDTag.TagType.Long,
                        Tag = e.UserState.ToString()
                    });

                    listener.TagRead(new RFIDTag
                    {
                        RFIDTagType = RFIDTag.TagType.Short,
                        Tag = e.UserState.ToString()
                    });
                }
            }
        }

        public override void Dispose()
        {
            Debug.WriteLine("Fake RFID Reader disposed");
        }
    }
}
