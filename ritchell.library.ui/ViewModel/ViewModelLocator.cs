/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:ritchell.library.ui.ViewModel"
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

namespace ritchell.library.ui.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {

            }
            else
            {
                //SetupRFIDReaders();
                SetupDebugRFID();
            }

            SimpleIoc.Default.Register<BookService>();
            SimpleIoc.Default.Register<BookCopyService>();
            SimpleIoc.Default.Register<SectionService>();
            SimpleIoc.Default.Register<LibraryUserService>();
            SimpleIoc.Default.Register<HolidayService>();
            SimpleIoc.Default.Register<HolidayPageViewModel>();
            SimpleIoc.Default.Register<UsersPageViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SectionPageViewModel>();
            SimpleIoc.Default.Register<BookPageViewModel>();
            SimpleIoc.Default.Register<BookCopyPageViewModel>();
        }

        private static void SetupDebugRFID()
        {
            RFIDGenratorDebug.MainWindow rfidDebugWindow = new RFIDGenratorDebug.MainWindow();

            SimpleIoc.Default.Register<IRFIDReader>(() => rfidDebugWindow.RFIDGeneratorShort, "short");
            SimpleIoc.Default.Register<IRFIDReader>(() => rfidDebugWindow.RFIDGeneratorLong, "long");

            rfidDebugWindow.Show();
        }

        private static void SetupRFIDReaders()
        {
            try
            {
                var shortReader = new ShortRangeRFID();
                var longReader = new LongRangeRFID();
                longReader.StartMonitoring();

                SimpleIoc.Default.Register<IRFIDReader>(() => shortReader, "short");
                SimpleIoc.Default.Register<IRFIDReader>(() => longReader, "long");
            }
            catch (System.Exception)
            {
            }
        }


        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public UsersPageViewModel UsersPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UsersPageViewModel>();
            }
        }


        public BookCopyPageViewModel BookCopyPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BookCopyPageViewModel>();
            }
        }

        public BookPageViewModel BookPageViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstanceWithoutCaching<BookPageViewModel>();
            }
        }

        public SectionPageViewModel SectionPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SectionPageViewModel>();
            }
        }

        public HolidayPageViewModel HolidayPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HolidayPageViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}