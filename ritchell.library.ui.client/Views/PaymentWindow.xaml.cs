using ritchell.library.model;
using ritchell.library.presentation.common.ViewServices;
using ritchell.library.ui.client.ViewModels;
using System;
using System.Windows;

namespace ritchell.library.ui.client.Views
{
    /// <summary>
    /// Interaction logic for UnpaidUnreturnedBooks.xaml
    /// </summary>
    public partial class PaymentWindow : Window, IWindow
    {
        public PaymentWindow()
        {
            InitializeComponent();
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
                DataContext = new PayablesViewModel((LibraryUser)value);
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
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        { 
            e.Cancel = true;
            CloseWindow();
        }
    }
}
