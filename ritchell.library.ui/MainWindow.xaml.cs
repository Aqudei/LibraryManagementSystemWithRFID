using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using ritchell.library.ui.ViewModel;

namespace ritchell.library.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            SimpleIoc.Default.Register<NavigationService>(() => this.LayoutRoot.NavigationService);
        }
    }
}