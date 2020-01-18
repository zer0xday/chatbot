using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Core
{
    public class PluginValidator
    {
        private string[] requiredMethods = new string[4]
        {
            "IsConnected",
            "SendMessage",
            "GetMessage",
            "Init"
        };

        private object[] GetPluginsArray()
        {
            object[] plugins = new object[]
            {
                new PolfanConnector.Plugin()
            };

            return plugins;
        }
      
        public bool Validate()
        {
            object[] plugins = GetPluginsArray();
 
            foreach (object plugin in plugins)
            {
                Type type = plugin.GetType();
                string pluginName = plugin.ToString();

                // Get the public methods
                MethodInfo[] arrayMethodsInfo = type.GetMethods(
                    BindingFlags.Public 
                    | BindingFlags.Instance 
                    | BindingFlags.DeclaredOnly
                );

                try
                {
                    ValidateMethodsName(arrayMethodsInfo, pluginName);
                    ValidateMethodsQty(arrayMethodsInfo, pluginName);
                } catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }

            return true;
        }

        private bool ValidateMethodsQty(MethodInfo[] methodsInfoArray, string pluginName)
        {
            if (methodsInfoArray.Length < requiredMethods.Length)
            {
                throw new Exception($"Methods quantity does not match in {pluginName}");
            }

            return true;
        }

        private bool ValidateMethodsName(MethodInfo[] methodsInfoArray, string pluginName)
        {
            foreach (MethodInfo methodInfo in methodsInfoArray)
            {
                if (!requiredMethods.Contains(methodInfo.Name))
                {
                    throw new Exception($"Methods does not match in {pluginName}");
                }
            }

            return true;
        }
    }
}
