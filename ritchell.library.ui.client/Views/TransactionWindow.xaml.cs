using ritchell.library.model;
using ritchell.library.presentation.common.ViewServices;
using ritchell.library.ui.client.ViewModels;
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
    /// Interaction logic for TransactionWindow.xaml
    /// </summary>
    public partial class TransactionWindow : Window, IWindow
    {
        public TransactionWindow()
        {
            InitializeComponent();
            this.Closing += (s, e) =>
            {
                e.Cancel = true;
                CloseWindow();
            };
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
                DataContext = new TransactionsViewModel((LibraryUser)value);
            }
        }

        public void CloseWindow()
        {
            this.Hide();
        }

        void IWindow.ShowDialog()
        {
            this.ShowDialog();
        }
    }
}
