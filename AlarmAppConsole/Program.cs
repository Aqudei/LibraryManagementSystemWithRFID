using ritchell.library.infrastructure.Hardware;
using ritchell.library.model.Services;
using System;
using System.Threading;
using System.Windows.Media;

namespace AlarmAppConsole
{
    class Program
    {
        private static MediaPlayer mediaPlayer;

        static void Main(string[] args)
        {
            //DataContext = this;

            mediaPlayer = new MediaPlayer();

            var soundFile = System.IO.Path.Combine(Environment.CurrentDirectory, "Car Alarm 02.wav");

            mediaPlayer.Open(new Uri(soundFile));

            var unborrowedMonitor = new AlarmApp.Services.UnborrowedBookMonitor(
                new BookCopyService(),
                new LongRangeRFID());

            unborrowedMonitor.UnborrowedIsGoingOut += UnborrowedMonitor_UnborrowedIsGoingOut;

            while (true)
            {
                Console.WriteLine("Monitoring is On.");
                Thread.Sleep(32);
            }
        }

        private static void UnborrowedMonitor_UnborrowedIsGoingOut(object sender, AlarmApp.Models.BookCopyWithInfo e)
        {
            Console.WriteLine("The book \"{0}\" with RFID {1} is going without being borrowed!", e.BookInfo.BookTitle, e.BookCopy.BookTagLong);
            mediaPlayer.Play();
        }
    }
}
