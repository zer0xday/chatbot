using System;
using System.Reflection;
using System.Text;

namespace Core
{
    public class Core
    {
        private const int IO_PLUGIN_INIT_TIMEOUT_SEC = 10;
        private const string pluginFolder = @"plugins\";
        private const string pluginExtension = @".dll";

        public string Init(string plugin)
        {
            dynamic pluginObject;

            // fixme
            try
            {
                pluginObject = LoadPlugin(plugin);
                var pluginValidator = new PluginValidator();
                pluginValidator.Validate(pluginObject);
            }
            catch (TimeoutException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }

            // temp
            pluginObject.Init("Testowy");

            return "Loaded";
        }

        public object LoadPlugin(string pluginName)
        {
            var endTime = DateTime.Now.AddSeconds(IO_PLUGIN_INIT_TIMEOUT_SEC);

            var pluginAssembly = GetPluginAssembly(pluginName);
            dynamic pluginInstance = GetPluginInstance(pluginAssembly, pluginName);

            //while (!pluginInstance.IsReady) // asynchronicznie?
            //{
            //    if (DateTime.Now >= endTime)
            //    {
            //        throw new TimeoutException($"IO plugin initializing timeout: {IO_PLUGIN_INIT_TIMEOUT_SEC}s");
            //    }
            //}

            return pluginInstance;
        }

        private string GetPluginLocation(string pluginName)
        {
            string baseDir = AppContext.BaseDirectory;
            string[] explodedPath = baseDir.Split("GuiClient");

            StringBuilder pluginPath = new StringBuilder();
            pluginPath.Append(explodedPath[0]);
            pluginPath.Append(pluginFolder);
            pluginPath.Append(pluginName);
            pluginPath.Append(pluginExtension);

            return pluginPath.ToString();
        }

        private Assembly GetPluginAssembly(string pluginName)
        {
            return Assembly.LoadFile(GetPluginLocation(pluginName));
        }

        private dynamic GetPluginInstance(Assembly pluginAssembly, string pluginName)
        {
            Type pluginType = pluginAssembly.GetType(pluginName + ".Plugin");
            dynamic pluginInstance = Activator.CreateInstance(pluginType);

            return pluginInstance;
        }
    }
}
