using System.Windows;

namespace ritchell.library.ui.client.ViewServices
{
    public interface IWindowNavigationService
    {
        void Show(string key);
        void ShowDialog(string key);
        void Add(string key, Window window);
    }
}
