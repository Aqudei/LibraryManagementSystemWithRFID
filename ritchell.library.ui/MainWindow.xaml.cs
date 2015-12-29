using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using ritchell.library.ui.View.DialogInterface;
using ritchell.library.ui.ViewModel;

namespace ritchell.library.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IRFIDManagerDialog
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();


            Loaded += (s, e) =>
            {
                SimpleIoc.Default.Register<IRFIDManagerDialog>(() => this);
            };

            Closing += (s, e) => ViewModelLocator.Cleanup();

            SimpleIoc.Default.Register<NavigationService>(() => this.LayoutRoot.NavigationService);
        }

        public void Manage(model.BookInfo bookInfo)
        {
            var dlg = new View.BookCopyPage();
            (dlg.DataContext as BookCopyPageViewModel).CurrentBook = bookInfo;
            dlg.ShowDialog();
        }
    }
}