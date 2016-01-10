/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ritchell.library.ui.client.ViewModels"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ritchell.library.model.Services;

namespace ritchell.library.ui.client.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                // SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<BorrowReturnBookViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LibraryUserService>();
            SimpleIoc.Default.Register<DashboarViewModel>();
            SimpleIoc.Default.Register<AuthenticationViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public AuthenticationViewModel LoginPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthenticationViewModel>();
            }
        }

        public DashboarViewModel DashboardPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DashboarViewModel>();
            }
        }

        public BorrowReturnBookViewModel BorrowReturnBookPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BorrowReturnBookViewModel>();
            }
        }
    }
}