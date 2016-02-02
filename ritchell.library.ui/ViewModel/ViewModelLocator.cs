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
using System.Diagnostics;
using ritchell.library.model.LibraryTransactions;
using ritchell.library.presentation.common.ViewServices;
using ritchell.library.ui.View;

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
                SetupRFIDReaders();
                //SetupDebugRFID();
            }

            SimpleIoc.Default.Register<DepartmentService>();
            SimpleIoc.Default.Register<BookService>();
            SimpleIoc.Default.Register<BookCopyService>();
            SimpleIoc.Default.Register<SectionService>();
            SimpleIoc.Default.Register<LibraryUserService>();
            SimpleIoc.Default.Register<HolidayService>();
            SimpleIoc.Default.Register<PaymentService>();
            SimpleIoc.Default.Register<CourseService>();

            SimpleIoc.Default.Register<HolidayPageViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SectionPageViewModel>();
            SimpleIoc.Default.Register<BookPageViewModel>();
            SimpleIoc.Default.Register<BookCopyPageViewModel>();
            SimpleIoc.Default.Register<DepartmentsViewModel>();
            SimpleIoc.Default.Register<PayablesViewModel>();
            SimpleIoc.Default.Register<CoursesViewModel>();
            SimpleIoc.Default.Register<UsersPageViewModel>();
            SimpleIoc.Default.Register<BorrowedBooksManagerViewModel>();
            //SetupWindows();
        }

        private static void SetupWindows()
        {
            WindowNavigationService wns = new WindowNavigationService();
            wns.Add(WindowNames.MainWindow, new MainWindow());
            SimpleIoc.Default.Register<IWindowNavigationService>(() => wns);
        }

        private static void SetupRFIDReaders()
        {
            try
            {
                var shortReader = new ShortRangeRFID();
                SimpleIoc.Default.Register<IRFIDReader>(() => shortReader, "short");
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("Hardware failure\nOpting to use short Null readers" + ex.Message);
                SimpleIoc.Default.Unregister<IRFIDReader>();
                SimpleIoc.Default.Register<IRFIDReader>(() => new NullRFID(), "short");

            }

            try
            {
                var longReader = new LongRangeRFID();
                SimpleIoc.Default.Register<IRFIDReader>(() => longReader, "long");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Hardware failure\nOpting to use long Null readers" + ex.Message);
                SimpleIoc.Default.Unregister("long");
                SimpleIoc.Default.Register<IRFIDReader>(() => new NullRFID(), "long");
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

        public CoursesViewModel CoursesViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstanceWithoutCaching<CoursesViewModel>();
            }
        }

        public BorrowedBooksManagerViewModel BorrowedBooksManagerViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstanceWithoutCaching<BorrowedBooksManagerViewModel>();
            }
        }

        public UsersPageViewModel UsersPageViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstanceWithoutCaching<UsersPageViewModel>();
            }
        }

        public PayablesViewModel PayablesViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstanceWithoutCaching<PayablesViewModel>();
            }
        }

        public DepartmentsViewModel DepartmentsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DepartmentsViewModel>();
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