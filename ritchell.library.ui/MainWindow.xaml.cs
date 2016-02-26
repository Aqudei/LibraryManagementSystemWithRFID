using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.ui.View.DialogInterface;
using ritchell.library.ui.ViewModel;
using ritchell.library.ui.Services;
using System.Windows.Controls;
using ritchell.library.reporting;
using ritchell.library.presentation.common.ViewServices;
using System;
using GalaSoft.MvvmLight.Views;
using System.Threading.Tasks;

namespace ritchell.library.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,
        IRFIDManagerDialog, IReportDialogParent, IWindow, IDialogService
    {
        ReportViewerService _ReportViewerService;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                SimpleIoc.Default.Register<IDialogService>(() => this);
                SimpleIoc.Default.Register<IRFIDManagerDialog>(() => this);
                SimpleIoc.Default.Register<IReportDialogParent>(() => this);
                SimpleIoc.Default.Register(() => LayoutRoot.NavigationService);
            };

            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        public void Manage(model.BookInfo bookInfo)
        {
            var dlg = new View.BookCopyPage();
            (dlg.DataContext as BookCopyPageViewModel).CurrentBook = bookInfo;
            dlg.ShowDialog();
        }

        public ReportViewerService ReportViewerService
        {
            get
            {
                return _ReportViewerService = _ReportViewerService ?? new ReportViewerService();
            }
        }

        public object WindowParam
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void ShowReport(object reportName)
        {
            var rptName = (reportName as string).ToLower();
            if (rptName.Contains("book"))
            {
                ReportViewerService.ShowBookListReport();
            }
            else if (rptName.Contains("clearance"))
            {
                ReportViewerService.ShowForClearanceReport();
            }
            else if (rptName.Contains("patron"))
            {
                ReportViewerService.ShowPatronsReport();
            }
            else if (rptName.Contains("most"))
            {
                ReportViewerService.ShowMostBOrrowed();
            }
            else if (rptName.Contains("payment"))
            {
                ReportViewerService.ShowPaymentsReport();
            }
            else if (rptName.Contains("log"))
            {
                ReportViewerService.ShowLogsReport();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var answer = MessageBox.Show("Do you really want to quit the application?", "Confirm Exit", MessageBoxButton.YesNo);
            e.Cancel = answer == MessageBoxResult.No;
        }

        void IWindow.ShowDialog()
        { }

        public void CloseWindow()
        { }

        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task ShowMessage(string message, string title)
        {
            throw new NotImplementedException();
        }

        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            var rslt = MessageBox.Show(message, title, MessageBoxButton.YesNo);
            afterHideCallback(rslt == MessageBoxResult.Yes);
            return new Task<bool>(() => rslt == MessageBoxResult.Yes);
        }

        public Task ShowMessageBox(string message, string title)
        {
            MessageBox.Show(message, title);
            return null;
        }
    }
}