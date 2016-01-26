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
        

        public event EventHandler<string> TagRead;

        public FakeRFID()
        {
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.WorkerSupportsCancellation = true;
        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (bgWorker.CancellationPending == false)
            {
                Thread.Sleep(2000);
                bgWorker.ReportProgress(0, Guid.NewGuid().ToString());
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
