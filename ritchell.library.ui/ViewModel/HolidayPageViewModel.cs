using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ritchell.library.model;
using ritchell.library.model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.ui.ViewModel
{
    public class HolidayPageViewModel : ViewModelBase
    {

        ObservableCollection<Holiday> _Holidays;


        public HolidayPageViewModel(HolidayService holidayService)
        {
            _HolidayService = holidayService;

            Holidays = new ObservableCollection<Holiday>(_HolidayService.GetHolidays());
        }

        private RelayCommand<DateTime> _AddHoliday;

        /// <summary>
        /// Gets the AddHoliday.
        /// </summary>
        public RelayCommand<DateTime> AddHoliday
        {
            get
            {
                return _AddHoliday
                    ?? (_AddHoliday = new RelayCommand<DateTime>(
                    p =>
                    {
                        try
                        {
                            var newHoliday = new Holiday
                            {
                                Day = p,
                                Description = HolidayDescription
                            };
                            _HolidayService.AddHoliday(newHoliday);
                            Holidays.Add(newHoliday);
                        }
                        catch (Exception)
                        {
                            Debug.WriteLine("ErrorOnAddHoliday");
                        }
                    },
                    p => p != DateTime.MinValue));
            }
        }

        private RelayCommand<Holiday> _DeleteHoliday;
        private HolidayService _HolidayService;

        /// <summary>
        /// Gets the AddHoliday.
        /// </summary>
        public RelayCommand<Holiday> DeleteHoliday
        {
            get
            {
                return _DeleteHoliday
                    ?? (_DeleteHoliday = new RelayCommand<Holiday>(
                    holiday =>
                    {
                        _HolidayService.DeleteHoliday(holiday);
                        Holidays.Remove(holiday);
                    },
                    holiday => holiday != null));
            }
        }

        public ObservableCollection<Holiday> Holidays
        {
            get
            {
                return _Holidays; ;
            }

            set
            {
                _Holidays = value;
                RaisePropertyChanged(() => Holidays);
            }
        }

        private string _HolidayDescription;

        public string HolidayDescription
        {
            get { return _HolidayDescription; }
            set
            {
                _HolidayDescription = value;
                RaisePropertyChanged(() => HolidayDescription);
            }
        }


        private RelayCommand _MarkWeekendsAsHolidays;
        public RelayCommand MarkWeekendsAsHolidays
        {
            get
            {
                return _MarkWeekendsAsHolidays = _MarkWeekendsAsHolidays ?? new RelayCommand(
                async () =>
                {
                    await Task.Run(() => _HolidayService.MarkSatSunAsHolidays());
                    Holidays = new ObservableCollection<Holiday>(_HolidayService.GetHolidays());
                }, () => true);
            }
        }

    }
}
