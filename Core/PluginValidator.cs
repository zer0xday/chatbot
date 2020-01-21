using System;
using System.Reflection;

namespace ChatBot
{
    public class PluginValidator
    {
        private readonly string[] requiredMethods = new string[4]
        {
            "SendMessage",
            "GetMessage",
            "Init",
            "End"
        };

        private readonly string[] requiredProperties = new string[1]
        {
            "IsReady"
        };

        public void Validate(object plugin)
        {
            Type type = plugin.GetType();
            string pluginName = plugin.ToString();

            ValidateMethods(type, pluginName);
            ValidateProperties(type, pluginName);
        }

        private void ValidateMethods(Type type, string pluginName)
        {
            foreach (var methodName in requiredMethods)
            {
                MethodInfo info = type.GetMethod(methodName);

                if (info == null || !info.IsPublic)
                {
                    throw new PluginMissingComponentException(pluginName, "method", methodName);
                }
            }
        }

        private void ValidateProperties(Type type, string pluginName)
        {
            foreach (var propertyName in requiredProperties)
            {
                PropertyInfo info = type.GetProperty(propertyName);

                if (info == null)
                {
                    throw new PluginMissingComponentException(pluginName, "property", propertyName);
                }
            }
        }
    }

    public class PluginMissingComponentException : MissingMethodException {
        public PluginMissingComponentException(string pluginName, string cmpName, string componentName) 
            : base($"Plugin {pluginName} does not contain required {cmpName}: {componentName}") { }
    }
}
