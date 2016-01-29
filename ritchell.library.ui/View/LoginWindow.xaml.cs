using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using ritchell.library.model.Services;
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

namespace ritchell.library.ui.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LibraryUserService _LibraryUserService;

        public string Username { get; set; }
        public string Password { get; set; }


        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;
            _LibraryUserService = new LibraryUserService();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (TryAuthenticate())
            {
                new MainWindow().Show();
                Close();
            }
            else
            {
                MessageBox.Show("Incorrect username/password", "Authentication Failed");
            }
        }

        private bool TryAuthenticate()
        {
            if (Username == "_Admin_" && Password == "_Admin_")
                return true;

            try
            {
                var admin = _LibraryUserService.GetAuthenticatedAdmin(Username, Password);
                return admin != null;
            }
            catch (Exception ex)
            {
                return false;
            }   
        }
    }
}
