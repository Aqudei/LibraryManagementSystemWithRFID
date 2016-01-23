using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ritchell.library.model;
using ritchell.library.model.LibraryTransactions;

namespace ritchell.library.ui.client.ViewServices
{
    public class WindowNavigationService : IWindowNavigationService
    {
        public class WindowNaviEvent : EventArgs
        {
            public enum WindowAction
            {
                Opened, Closed
            }

            public WindowAction WindowActionType { get; set; }
            public string WindowKey { get; set; }
        }

        Dictionary<string, Window> Windows;
        private PaymentService _PaymentService;

        public event EventHandler<WindowNaviEvent> WindowNaviEventHander;
        public WindowNavigationService()
        {
            Windows = new Dictionary<string, Window>();
            _PaymentService = new PaymentService();
        }

        public void Show(string key)
        {
            if (Windows.ContainsKey(key))
            {
                Windows[key].Show();
                RaiseEvent(key, WindowNaviEvent.WindowAction.Opened);
            }
        }

        private void RaiseEvent(string key, WindowNaviEvent.WindowAction windowAction)
        {
            var handler = WindowNaviEventHander;
            if (handler != null)
                handler(this, new WindowNaviEvent
                {
                    WindowActionType = windowAction,
                    WindowKey = key
                });
        }

        public void ShowDialog(string key)
        {
            if (Windows.ContainsKey(key))
            {
                Windows[key].ShowDialog();
                RaiseEvent(key, WindowNaviEvent.WindowAction.Closed);
            }
        }

        public void Add(string key, Window window)
        {
            if (Windows.ContainsKey(key) == false)
                Windows.Add(key, window);
        }

        public void ShowPaymentsOf(LibraryUser currentUser)
        {
            var payables = _PaymentService.GetPayableTransactions(currentUser);
            var vm = new ViewModels.PayablesViewModel(payables);
            var payableWindos = new Views.PaymentWindow();

            payableWindos.DataContext = vm;
            payableWindos.ShowDialog();
            payableWindos = null;
        }
    }
}
