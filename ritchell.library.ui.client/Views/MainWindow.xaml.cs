using GalaSoft.MvvmLight.Views;
using System.Windows;
using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using System.Windows.Threading;

namespace ritchell.library.ui.client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDialogService
    {
        public MainWindow()
        {
            InitializeComponent();

            SimpleIoc.Default.Register<IDialogService>(() => this);
        }

        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task ShowMessage(string message, string title)
        {
            textBlockMessage.Text = message;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(4);
            timer.Tick += (s, e) =>
            {
                textBlockMessage.Text = "";
                textBlockMessage.Visibility = Visibility.Collapsed;
                (s as DispatcherTimer).Stop();
            };
            textBlockMessage.Visibility = Visibility.Visible;
            timer.Start();
            return null;
        }

        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task ShowMessageBox(string message, string title)
        {
            MessageBox.Show(message, title);
            return null;
        }
    }
}
