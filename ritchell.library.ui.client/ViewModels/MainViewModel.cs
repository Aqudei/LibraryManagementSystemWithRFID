using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.ui.client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _CurrentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _CurrentViewModel = _CurrentViewModel ?? SimpleIoc.Default.GetInstance<LoginPageViewModel>(); }
            set
            {
                _CurrentViewModel = value;
                RaisePropertyChanged(() => CurrentViewModel);
            }
        }

        public MainViewModel()
        { }

    }
}
