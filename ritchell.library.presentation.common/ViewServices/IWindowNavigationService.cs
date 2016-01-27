using System.Windows;

namespace ritchell.library.presentation.common.ViewServices
{
    public interface IWindowNavigationService
    {
        void Show(string key, object parameter);
        void ShowDialog(string key, object parameter);
        void Add(string key, IWindow window);
    }
}
