using System;
using PolfanConnector;

namespace Core
{
    public class Core
    {
        private const int IO_PLUGIN_INIT_TIMEOUT_SEC = 10;

        private dynamic pluginInstance;

        public Core()
        {
            PluginValidator pluginValidator = new PluginValidator();

            // Validate referenced plugins
            pluginValidator.Validate();

            
        }

        public void LoadPlugin()
        {
            var endTime = DateTime.Now.AddSeconds(IO_PLUGIN_INIT_TIMEOUT_SEC);

            //później trzeba wywalić PolfanConnector z odwołań - ma się ładować na żądanie wg konfiguracji
            pluginInstance = new Plugin();
            pluginInstance.Init();

            while (!pluginInstance.IsReady) // asynchronicznie?
            {
                if (DateTime.Now >= endTime)
                {
                    throw new TimeoutException($"IO plugin initializing timeout: {IO_PLUGIN_INIT_TIMEOUT_SEC}s");
                }
            }
        }
    }
}
