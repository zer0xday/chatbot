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
    public partial class MainWindow : Window
    {
        private Core core;
        private const string CONNECTED = "Connected";
        private const string DISCONNECTED = "Disconnected";
        private const string BTN_CONNECT = "Connect";
        private const string BTN_DISCONNECT = "Disconnect";
        private readonly string USED_PLUGIN_NAME;
        private string chatBoxContent = "<meta charset=utf-8>";
        private string[] config
        {
            get => ConfigurationManager.AppSettings.AllKeys;
        }

        public MainWindow()
        {
            InitializeComponent();

            // initialize core instance
            core = new Core();

            // get used plugin
            var fileDialog = new FileDialog();
            fileDialog.ShowDialog();

            USED_PLUGIN_NAME = fileDialog.pluginName;

            core.OnSystemMessage = (string message) =>
            {
                chatBoxContent += message + "<br>";

                chatBox.Dispatcher.Invoke(() =>
                {
                    chatBox.NavigateToString(chatBoxContent);
                });
            };
        }

        private void ConnectButton_Handler(object sender, RoutedEventArgs e)
        {
            var nameDialog = new NameDialog();
            nameDialog.ShowDialog();

            core.Connect(
                nameDialog.BotName,
                USED_PLUGIN_NAME
            );

            ChangeConnectionStatus();
        }

        private void OnEnterKeyDown_Handler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                chatBoxContent += message.Text;
                message.Text = "";
                core.SendMessage(message.Text);
            }
        }

        private void ChangeConnectionStatus()
        {
            if (connectionStatus.Text == CONNECTED)
            {
                connectBtn.Content = BTN_CONNECT;
                connectionStatus.Text = DISCONNECTED;
                connectionStatus.Foreground = new SolidColorBrush(Colors.Red);
            } else
            {
                connectBtn.Content = BTN_DISCONNECT;
                connectionStatus.Text = CONNECTED;
                connectionStatus.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
    }
}
