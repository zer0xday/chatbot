#define debuger

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using PolfanConnector;

namespace Core
{
    public class PuginValidator
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

            Type myType = (typeof(PolfanConnector.Plugin));
            // Get the public methods.
            MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            Console.WriteLine("\nThe number of public methods is {0}.", myArrayMethodInfo.Length);
            // Display information for all methods.
            DisplayMethodInfo(myArrayMethodInfo);

            return true;
        }

        private void DisplayMethodInfo(MethodInfo[] methodsInfoArray)
        {
            Console.WriteLine("Required methods: ");
            foreach(string method in requiredMethods)
            {
                Console.WriteLine(method);
            }

            Console.WriteLine("\nAvailable methods: ");
            foreach (MethodInfo methodInfo in methodsInfoArray)
            {
                Console.WriteLine(methodInfo.Name);
            }
        }
    }
}
