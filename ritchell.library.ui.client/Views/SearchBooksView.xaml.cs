using ritchell.library.presentation.common.ViewServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ritchell.library.ui.client.Views
{
    /// <summary>
    /// Interaction logic for SearchBooksView.xaml
    /// </summary>
    public partial class SearchBooksView : Window, IWindow
    {
        public SearchBooksView()
        {
            InitializeComponent();
            DataContext = new ViewModels.SearchBooksViewModel();
        }

        private object _WindowParam;

        public object WindowParam
        {
            get
            {
                return _WindowParam;
            }

            set
            {
                _WindowParam = value;
            }
        }

        public void CloseWindow()
        {
            Hide();
        }

        void IWindow.ShowDialog()
        {
            ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            CloseWindow();
        }
    }
}
