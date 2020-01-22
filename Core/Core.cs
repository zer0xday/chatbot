using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatBot
{
    public partial class Core
    {
        public Action<bool> OnStateChange = (bool state) => { };
        public Action<string> OnSystemMessage = (string message) => { };

        private const int IO_PLUGIN_INIT_TIMEOUT_SEC = 10;

        private dynamic pluginInstance;
        private string botName;

        public void Connect(string botName, string pluginPath)
        {
            this.pluginInstance = PluginLoader.LoadPlugin(pluginPath);
            this.botName = botName;

            var pluginValidator = new PluginValidator();
            pluginValidator.Validate(pluginInstance);

            initPlugin();
        }

        public void Disconnect()
        {
            pluginReadyOrFail();

            pluginInstance.End();
        }

        public void SendMessage(string message)
        {
            pluginReadyOrFail();

            pluginInstance.SendMessage(message);
            writeSystemMessage($"Bot: {message}");
        }

        private async void initPlugin()
        {
            await Task.Run(() => {
                var startTime = new DateTime();

                pluginInstance.Init(this.botName);

                while(!pluginInstance.IsReady) {
                    if ((new DateTime()).Subtract(startTime).TotalSeconds >= IO_PLUGIN_INIT_TIMEOUT_SEC)
                    {
                        writeSystemMessage($"Cannot load input/output plugin: {IO_PLUGIN_INIT_TIMEOUT_SEC} seconds timeout was exceeded.");
                        OnStateChange(false);
                        return;
                    }
                }

                writeSystemMessage("The input/output plugin has successfully initialized.");
                OnStateChange(true);
                runMainLoop();
            });
        }

        private async void runMainLoop()
        {
            await Task.Run(() => {
                writeSystemMessage("The conversation has begun.");

                while (pluginInstance.IsReady)
                {
                    Tuple<string, string> message = pluginInstance.GetMessage();

                    if (message == null)
                    {
                        continue;
                    }

                    writeSystemMessage($"{message.Item1}: {message.Item2}");

                    var response = processPharse(message.Item1, message.Item2);
                    pluginInstance.SendMessage(response);

                    writeSystemMessage($"Bot: {response}");
                }

                writeSystemMessage("The conversation has ended.");
                OnStateChange(false);
            });
        }

        private string processPharse(string userName, string message)
        {
            var regex = new Regex("hej|cześć|elo|witam");
            var match = regex.Match(message);

            return match.Captures.Count > 0 ? $"siemka tasiemka {userName}" : $"{userName} słuchaj co Ci powiem mądrego, głupcze ;)";
        }

        private void writeSystemMessage(string text)
        {
            var time = DateTime.Now.ToString("hh:mm:ss");
            text = $"<b>[{time}]</b> {text}";

            OnSystemMessage(text);
        }

        private void pluginReadyOrFail()
        {
            if (pluginInstance == null || !pluginInstance.IsReady)
            {
                throw new PluginNotReadyException();
            }
        }

        public class PluginNotReadyException : Exception 
        { 
            public PluginNotReadyException() : base() { }
        }
    }
}
