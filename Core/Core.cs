using System;
using System.Collections.Generic;
using PolfanConnector;

namespace Core
{
    public class Core
    {
        static void Main()
        {
            PluginValidator pluginValidator = new PluginValidator();

            // Validate referenced plugins
            pluginValidator.Validate();
        }
    }
}
