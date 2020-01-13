#define debuger

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
      
        public bool Validate()
        {
#if (!debuger)
                    Console.SetOut(TextWriter.Null);
                    Console.SetError(TextWriter.Null);
#endif

            bool validated = true;

            object[] plugins = new object[] 
            {
                new PolfanConnector.Plugin()
            };
 
            foreach (object plugin in plugins)
            {
                Type type = plugin.GetType();

                // Get the public methods
                MethodInfo[] arrayMethodsInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                if (!ValidateMethodsQty(arrayMethodsInfo))
                {
                    Console.WriteLine("Methods quantity does not match in {0}", plugin.ToString());

                    validated = false;
                    break;
                }

                // Display information for all methods
                if (!ValidateMethodsName(arrayMethodsInfo))
                {
                    Console.WriteLine("Methods does not match in {0}", plugin.ToString());

                    validated = false;
                    break;
                }
            }

            return validated;
        }

        private bool ValidateMethodsQty(MethodInfo[] methodsInfoArray)
        {
            if (methodsInfoArray.Length < requiredMethods.Length)
            {
                return false;
            }

            return true;
        }

        private bool ValidateMethodsName(MethodInfo[] methodsInfoArray)
        {
            bool passed = true;

            foreach (MethodInfo methodInfo in methodsInfoArray)
            {
                if (!requiredMethods.Contains(methodInfo.Name))
                {
                    passed = false;
                    break;
                }
            }

            return passed;
        }
    }
}
