using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Hardware
{
    public class FakeRFID : IRFIDReader
    {
        private BackgroundWorker bgWorker;

        Guid[] FakeGuids = new Guid[3];

        public event EventHandler<string> TagRead;

        public FakeRFID()
        {
            FakeGuids[0] = Guid.Parse("{F7561AF5-0C6D-4A8A-B925-2D6AB2B673A9}");
            FakeGuids[1] = Guid.Parse("{B5200174-814F-432B-A1F7-7FF5CC2D90D5}");
            FakeGuids[2] = Guid.Parse("{FD8476BA-5EEC-4FDF-AF7F-D50778E77D59}");

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
            var handler = TagRead;
            if (handler != null)
                handler(this, e.UserState as string);
        }

        public void Dispose()
        { }

        public void StartReader()
        {
            bgWorker.RunWorkerAsync();
        }

        public void StopReader()
        {
            bgWorker.CancelAsync();
        }
    }
}
