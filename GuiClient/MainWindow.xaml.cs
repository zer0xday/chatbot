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
        private readonly string USED_PLUGIN_NAME;

        private string[] config
        {
            get => ConfigurationManager.AppSettings.AllKeys;
        }

        public MainWindow()
        {
            InitializeComponent();

            // initialize core instance
            core = new Core();
            // get 
            var fileDialog = new FileDialog();
            fileDialog.ShowDialog();

            USED_PLUGIN_NAME = fileDialog.pluginName;
        }

        private void ConnectButton_Handler(object sender, RoutedEventArgs e)
        {
            var dialog = new NameDialog();
            dialog.ShowDialog();
            var result = core.Init(USED_PLUGIN_NAME);

            chatBox.NavigateToString($"<p style='color:red'>{USED_PLUGIN_NAME}</p>");

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
