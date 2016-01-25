using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.ui.View.DialogInterface;
using ritchell.library.ui.ViewModel;
using ritchell.library.ui.Services;
using System.Windows.Controls;
using ritchell.library.reporting;

namespace ritchell.library.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,
        IRFIDManagerDialog, IReportDialogParent
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
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var answer = MessageBox.Show("Do you really want to quit the application?", "Confirm Exit", MessageBoxButton.YesNo);
            e.Cancel = answer == MessageBoxResult.No;
        }
    }
}