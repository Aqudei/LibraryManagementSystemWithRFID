using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AlarmApp.Services
{
    public class UnborrowedBookMonitor
    {
        public event EventHandler<Models.BookCopyWithInfo> UnborrowedIsGoingOut;

        private DispatcherTimer monitorTimer;

        public UnborrowedBookMonitor()
        {
            monitorTimer = new DispatcherTimer();
            monitorTimer.Interval = TimeSpan.FromMilliseconds(200);
            monitorTimer.Tick += monitorTimer_Tick;
            monitorTimer.IsEnabled = true;
            monitorTimer.Start();
        }

        void monitorTimer_Tick(object sender, EventArgs e)
        {
            var randomGenerator = new Random(DateTime.Now.Millisecond);

            if ((randomGenerator.Next(100) % 100) == 0)
            {
                var handler = UnborrowedIsGoingOut;
                if (handler != null)
                {
                    handler(this, null);
                    Console.WriteLine("Unborrowed Book!");
                }
            }
        }
    }
}
