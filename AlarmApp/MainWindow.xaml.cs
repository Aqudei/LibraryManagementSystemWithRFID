using ritchell.library.infrastructure.Hardware;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                DataContext = this;

                mediaPlayer = new MediaPlayer();

                var soundFile = System.IO.Path.Combine(Environment.CurrentDirectory, "Car Alarm 02.wav");

                mediaPlayer.Open(new Uri(soundFile));
                mediaPlayer.Volume = 100;
                mediaPlayer.MediaEnded += (ss, ee) =>
                {
                    mediaPlayer.Position = TimeSpan.FromMilliseconds(1);
                };

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
            mediaPlayer.Play();
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
