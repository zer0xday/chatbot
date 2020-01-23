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
        private const string CONNECTED = "Połączony";
        private const string DISCONNECTED = "Rozłączony";
        private const string BTN_CONNECT = "Połącz";
        private const string BTN_DISCONNECT = "Rozłącz";
        private readonly string USED_PLUGIN_NAME;
        private string chatBoxContent = "<meta charset=utf-8>";
        private string[] config
        {
            get => ConfigurationManager.AppSettings.AllKeys;
        }

        public MainWindow()
        {
            InitializeComponent();

            core = new Core();

            USED_PLUGIN_NAME = GetUsedPlugin();

            // listen to messages
            core.OnSystemMessage = (string message) =>
            {
                chatBoxContent += message + "<br>";

                chatBox.Dispatcher.Invoke(() => chatBox.NavigateToString(chatBoxContent));
            };

            // listen to plugin state
            core.OnStateChange = (bool isReady) =>
            {
                connectionStatus.Dispatcher.Invoke(() => ChangeConnectionStatus(isReady));
            };
        }

        private void ConnectButton_Handler(object sender, RoutedEventArgs e)
        {
            var nameDialog = new NameDialog();
            nameDialog.ShowDialog();

            if (nameDialog.DialogResult == false)
            {
                return;
            }

            core.Connect(
                nameDialog.BotName,
                USED_PLUGIN_NAME
            );
        }

        private void ConnectToChat() { }

        private void OnEnterKeyDown_Handler(object sender, KeyEventArgs e)
        {
            string text = message.Text.Trim();
            message.Text = "";

            if (e.Key == Key.Return && text.Length > 0)
            {
                core.SendMessage(message.Text);    
            }
        }

        private string GetUsedPlugin()
        {
            var fileDialog = new FileDialog();
            fileDialog.ShowDialog();

            if (fileDialog.DialogResult == false)
            {
                App.Current.Shutdown();
                return null;
            }

            return fileDialog.pluginName;
        }

        private void ChangeConnectionStatus(bool isConnected)
        {
            if (isConnected)
            {
                connectBtn.Content = BTN_DISCONNECT;
                connectionStatus.Text = CONNECTED;
                connectionStatus.Foreground = new SolidColorBrush(Colors.Green);
            } else
            {
                connectBtn.Content = BTN_CONNECT;
                connectionStatus.Text = DISCONNECTED;
                connectionStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
