using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using ritchell.library.ui.View.DialogInterface;
using ritchell.library.ui.ViewModel;
using ritchell.library.ui.Services;
using System;
using System.Windows.Controls;

namespace ritchell.library.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,
        IRFIDManagerDialog, IReportDialogParent
    {
        reporting.Services.ReportViewerService _ReportViewerService;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //_ReportViewerService = new reporting.Services.ReportViewerService();

            Loaded += (s, e) =>
            {
                SimpleIoc.Default.Register<IRFIDManagerDialog>(() => this);
                SimpleIoc.Default.Register<IReportDialogParent>(() => this);
                SimpleIoc.Default.Register<NavigationService>(() => this.LayoutRoot.NavigationService);
            };

            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        public void Manage(model.BookInfo bookInfo)
        {
            var dlg = new View.BookCopyPage();
            (dlg.DataContext as BookCopyPageViewModel).CurrentBook = bookInfo;
            dlg.ShowDialog();
        }

        public UserControl Report { get; set; }

        public void ShowReport(object reportName)
        {
            var rptName = (reportName as string).ToLower();
            if (rptName.Contains("book"))
            {
                Report = _ReportViewerService.BookListReport();
            }
            else if (rptName.Contains("clearance"))
            {
                Report = _ReportViewerService.ForClearanceReport();
            }
            else if (rptName.Contains("patron"))
            {
                Report = _ReportViewerService.PatronsReport();
            }


            var newWin = new View.ReportViewer();
            newWin.DataContext = this;
            newWin.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var answer = MessageBox.Show("Do you really want to quit the application?", "Confirm Exit", MessageBoxButton.YesNo);
            e.Cancel = answer == MessageBoxResult.No;
        }
    }
}