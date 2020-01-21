using System;
using System.Reflection;
using System.Text;

namespace Core
{
    partial class Core
    {
        private class PluginLoader
        {
            private const string pluginFolder = @"plugins\";
            private const string pluginExtension = @".dll";

            public object LoadPlugin(string pluginName)
            {
                var pluginAssembly = GetPluginAssembly(pluginName);
                dynamic pluginInstance = GetPluginInstance(pluginAssembly, pluginName);

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
}
