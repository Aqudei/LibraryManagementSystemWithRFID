/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ritchell.library.ui.client.ViewModels"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ritchell.library.infrastructure.Hardware;
using ritchell.library.model.Services;
using System.Diagnostics;
using ritchell.library.presentation.common.ViewServices;
using ritchell.library.model.LibraryTransactions;

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
                SetupRealRFIDReaders();
            }

            SimpleIoc.Default.Register<LibraryUserService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PaymentService>();
            SimpleIoc.Default.Register<AuthenticationViewModel>();

            SetupWindows();
        }


        private static void SetupWindows()
        {
            WindowNavigationService wns = new WindowNavigationService();
            wns.Add(ViewServices.WindowNames.PaymentWindow, new Views.PaymentWindow());

            SimpleIoc.Default.Register<IWindowNavigationService>(() => wns);

        }

        private static void SetupRealRFIDReaders()
        {
            try
            {
                var shortReader = new ShortRangeRFID();
                SimpleIoc.Default.Register<IRFIDReader>(() => shortReader, "short");
            }
            catch (System.Exception)
            {
                Debug.WriteLine("Error  opening short rfid.");
            }
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

        public AuthenticationViewModel AuthenticationViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthenticationViewModel>();
            }
        }
    }
}