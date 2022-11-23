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

namespace CNA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Client m_client;
        public MainWindow(Client client)
        {
        }
        /*
        public MainWindow(Client client)
        {
            InitializeComponent();
            m_client = client;

        }
         */
        public void UpdateChatBox(string message)
        {

        }
        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
