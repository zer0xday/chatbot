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
using System.Configuration;
using Core;
using System.IO;

namespace GuiClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] config
        {
            get { return ConfigurationManager.AppSettings.AllKeys; }
            set { }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton(object sender, RoutedEventArgs e)
        {
            var core = new Core.Core();
            var result = core.Init(config[0]);

            rtb.AppendText(result);

            // TODO: button actual status
            ChangeConnectionStatus();
        }

        private void ChangeConnectionStatus()
        {
            const string CONNECTED = "Connected";
            const string DISCONNECTED = "Disconnected";

            if (connectionStatus.Text == CONNECTED)
            {
                connectionStatus.Text = DISCONNECTED;
            } else
            {
                connectionStatus.Text = CONNECTED;
            }
        }
    }
}
