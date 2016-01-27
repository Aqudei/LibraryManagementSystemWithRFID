using System;
using System.Collections.Generic;
using System.Windows;

namespace ritchell.library.presentation.common.ViewServices
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

        Dictionary<string, IWindow> Windows;

        public event EventHandler<WindowNaviEvent> WindowNaviEventHander;

        public WindowNavigationService()
        {
            Windows = new Dictionary<string, IWindow>();
        }

        public void Show(string key, object parameter)
        {
            if (Windows.ContainsKey(key))
            {
                ((IWindow)Windows[key]).WindowParam = parameter;
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

        public void ShowDialog(string key, object parameter)
        {
            if (Windows.ContainsKey(key))
            {
                ((IWindow)Windows[key]).WindowParam = parameter;
                Windows[key].ShowDialog();
                RaiseEvent(key, WindowNaviEvent.WindowAction.Closed);
            }
        }

        public void Add(string key, IWindow window)
        {
            if (Windows.ContainsKey(key) == false)
                Windows.Add(key, window);
        }
    }
}
