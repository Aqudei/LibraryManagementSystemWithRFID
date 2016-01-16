using GalaSoft.MvvmLight.Threading;
using ritchell.library.infrastructure.Hardware;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlarmApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer;
        public ICommand _StopAlarmCommand;
        private string soundFileUrl;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                DataContext = this;

                mediaPlayer = new MediaPlayer();
                soundFileUrl = System.IO.Path.Combine(Environment.CurrentDirectory, "Car Alarm 02.wav");



                var unborrowedMonitor = new Services.UnborrowedBookMonitor(
                    new BookCopyService(),
                    new LongRangeRFID());

                unborrowedMonitor.UnborrowedIsGoingOut += unborrowedMonitor_UnborrowedIsGoingOut;
            };
        }


        public ICommand StopAlarmCommand
        {
            get
            {
                return _StopAlarmCommand = _StopAlarmCommand ?? new StopAlarmCommandDef(this);
            }
        }

        void unborrowedMonitor_UnborrowedIsGoingOut(object sender, Models.BookCopyWithInfo e)
        {
            //Debug.WriteLine("UnborrowedBookGoindOut");
            //var now = DateTime.Now;
            //if ((now - lastTagReadTime).Seconds < 5)
            //    return;

            //Dispatcher.Invoke(new Action(() =>
            //{

            mediaPlayer.Open(new Uri(soundFileUrl));
            mediaPlayer.Play();

            //    lastTagReadTime = now;
            //}));
        }

        class StopAlarmCommandDef : ICommand
        {
            private MainWindow _DC;

            public StopAlarmCommandDef(MainWindow dc)
            {
                _DC = dc;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                _DC.mediaPlayer.Stop();
            }


            public event EventHandler CanExecuteChanged;
        }


    }


}
