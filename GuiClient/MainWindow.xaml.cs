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
        private string usedPluginPath;
        private string chatBoxContent = "<meta charset=utf-8>";
        private bool isReady = false;

        public MainWindow()
        {
            InitializeComponent();

            core = new Core();

            usedPluginPath = GetUsedPlugin();

            statusBarText.Text = "Gotowy";

            ListenToMessages();
            ListenToPluginStates();
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
                    usedPluginPath
                );
            }
        }

        private void ListenToMessages()
        {
            core.OnSystemMessage = (string message) =>
            {
                chatBoxContent += message + "<br>";
                chatBox.Dispatcher.Invoke(() => chatBox.NavigateToString(chatBoxContent));
            };
        }

        private void ListenToPluginStates()
        {
            core.OnStateChange = (bool isReady) => {
                this.isReady = isReady;
                ChangeConnectionStatus(isReady);
            };
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
            if (!isReady)
            {
                return;
            }

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

        private void changePluginButton_Click(object sender, RoutedEventArgs e)
        {
            if (isReady)
            {
                var result = MessageBox.Show(
                    "Obecnie trwająca rozmowa zostanie zakończona. Kontynuować?",
                    "Potwierdź zmianę pluginu",
                    MessageBoxButton.YesNo
                );

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                try
                {
                    core.Disconnect();
                } catch (Core.PluginNotReadyException) { }
            }

            usedPluginPath = GetUsedPlugin();
        }
    }
}
