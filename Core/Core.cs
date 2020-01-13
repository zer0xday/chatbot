using System;
using System.Collections.Generic;

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
