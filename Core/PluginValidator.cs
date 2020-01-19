using System;
using System.Reflection;
using System.Linq;

namespace Core
{
    public class PluginValidator
    {
        private readonly string[] requiredMethods = new string[4]
        {
            "IsConnected",
            "SendMessage",
            "GetMessage",
            "Init"
        };

        public string Validate(object plugin)
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
                return e.Message;
            }
            return "Plugin validated";
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
