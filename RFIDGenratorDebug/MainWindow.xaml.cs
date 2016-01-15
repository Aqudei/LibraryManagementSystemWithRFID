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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RFIDGenratorDebug
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RFIDShortLongDebug _RFIDGeneratorShort;
        private RFIDShortLongDebug _RFIDGeneratorLong;

        public RFIDShortLongDebug RFIDGeneratorShort
        {
            get
            {
                return _RFIDGeneratorShort;
            }

            set
            {
                _RFIDGeneratorShort = value;
            }
        }

        public RFIDShortLongDebug RFIDGeneratorLong
        {
            get
            {
                return _RFIDGeneratorLong;
            }

            set
            {
                _RFIDGeneratorLong = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            RFIDGeneratorShort = new RFIDShortLongDebug();
            RFIDGeneratorLong = new RFIDShortLongDebug();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            RFIDGeneratorShort.Generate();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            RFIDGeneratorLong.Generate();
        }
    }
}
