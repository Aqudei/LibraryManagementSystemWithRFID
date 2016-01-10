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
        private AuthenticationViewModel _AuthenticationViewModel;

        public AuthenticationViewModel AuthenticationViewModel
        {
            get
            {
                return _AuthenticationViewModel = _AuthenticationViewModel ?? SimpleIoc.Default.GetInstance<AuthenticationViewModel>();
            }
            set
            {
                _AuthenticationViewModel = value;
                RaisePropertyChanged(() => AuthenticationViewModel);
            }
        }

        public MainViewModel()
        {
           
        }

    }
}
