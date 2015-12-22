using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;

namespace ritchell.library.ui.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {

        }

        private RelayCommand<string> _NavigateToPageCommand;

        /// <summary>
        /// Gets the NavigateToCommand.
        /// </summary>
        public RelayCommand<string> NavigateToPageCommand
        {
            get
            {
                return _NavigateToPageCommand
                    ?? (_NavigateToPageCommand = new RelayCommand<string>(
                    (pageName) =>
                    {
                        GetNavService().Navigate(new System.Uri("../View/" + pageName, System.UriKind.RelativeOrAbsolute));
                    }));
            }
        }


        private NavigationService GetNavService()
        {
            return SimpleIoc.Default.GetInstance<NavigationService>();
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}