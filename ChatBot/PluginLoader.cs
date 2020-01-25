using System;
using System.Reflection;

namespace ChatBot
{
    partial class Core
    {
        private class PluginLoader
        {
            public const string MAIN_CLASS_NAME = "Plugin";

            public static object LoadPlugin(string pluginPath)
            {
                dynamic pluginInstance;

                try
                {
                    var pluginAssembly = GetPluginAssembly(pluginPath);
                    pluginInstance = GetPluginInstance(pluginAssembly);
                } 
                catch(Exception)
                {
                    throw new PluginLoadException(pluginPath);
                }

                return pluginInstance;
            }

            private static Assembly GetPluginAssembly(string pluginPath)
            {
                return Assembly.LoadFile(pluginPath);
            }

            private static string GetPluginName(Assembly pluginAssembly)
            {
                return pluginAssembly.GetName().Name;
            }

            private static dynamic GetPluginInstance(Assembly pluginAssembly)
            {
                Type pluginType = pluginAssembly.GetType(GetPluginName(pluginAssembly) + "." + MAIN_CLASS_NAME);
                return Activator.CreateInstance(pluginType);
            }

            public class PluginLoadException : Exception
            {
                public PluginLoadException(string pluginPath) 
                    : base($"Cannot load plugin: {pluginPath}") { }
            }
        }
    }
}
