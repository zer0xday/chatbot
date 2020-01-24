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
        private readonly string USED_PLUGIN_NAME;
        private string chatBoxContent = "<meta charset=utf-8>";
        private string[] config
        {
            get => ConfigurationManager.AppSettings.AllKeys;
        }
        private bool isReady = false;

        public MainWindow()
        {
            InitializeComponent();

            core = new Core();

            USED_PLUGIN_NAME = GetUsedPlugin();

            statusBarText.Text = "Gotowy";

            // listen to messages
            core.OnSystemMessage = (string message) =>
            {
                chatBoxContent += message + "<br>";
                chatBox.Dispatcher.Invoke(() => chatBox.NavigateToString(chatBoxContent));
            };

            // listen to plugin state
            core.OnStateChange = (bool isReady) => {
                this.isReady = isReady;
                ChangeConnectionStatus(isReady);
            };
        }

        private void ConnectButton_Handler(object sender, RoutedEventArgs e)
        {
            if (isReady)
            {
                core.Disconnect();
            } 
            else
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
        }

        private void OnEnterKeyDown_Handler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                sendMessageFromField();

            }
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            sendMessageFromField();
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
            connectBtn.Dispatcher.Invoke(() =>
            {
                connectBtn.Content = isConnected ? "Zakończ rozmowę" : "Rozpocznij rozmowę";
            });

            statusBarText.Dispatcher.Invoke(() => 
            { 
                statusBarText.Text = isConnected ? "W trakcie rozmowy" : "Rozmowa zakończona";
            });

            sendButton.Dispatcher.Invoke(() =>
            {
                sendButton.IsEnabled = isConnected;
            });
        }

        private void sendMessageFromField()
        {
            string text = message.Text.Trim();            
            message.Text = "";

            if (text.Length > 0)
            {
                try
                {
                    core.SendMessage(text);
                }
                catch (Core.PluginNotReadyException) { }
            }
        }
    }
}
