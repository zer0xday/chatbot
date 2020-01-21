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
using ChatBot;
using System.IO;

namespace GuiClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Core core;
        private const string CONNECTED = "Connected";
        private const string DISCONNECTED = "Disconnected";
        private readonly string GUI_CLIENT_FORM;
        private readonly string CONSOLE_CLIENT_FORM;

        private string[] config
        {
            get { return ConfigurationManager.AppSettings.AllKeys; }
            set { }
        }

        public MainWindow()
        {
            InitializeComponent();
            core = new Core();
            GUI_CLIENT_FORM = config[0];
            CONSOLE_CLIENT_FORM = config[1];
        }

        private void ConnectButton_Handler(object sender, RoutedEventArgs e)
        {
            var dialog = new NameDialog();
            dialog.ShowDialog();
            var result = core.Init(CONSOLE_CLIENT_FORM);

            rtb.AppendText($"Nick bota: {dialog.BotName}");

            // TODO: button actual status
            ChangeConnectionStatus();
        }

        private void ChangeConnectionStatus()
        {
            if (connectionStatus.Text == CONNECTED)
            {
                connectionStatus.Text = DISCONNECTED;
                connectionStatus.Foreground = new SolidColorBrush(Colors.Red);
            } else
            {
                connectionStatus.Text = CONNECTED;
                connectionStatus.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
    }
}
