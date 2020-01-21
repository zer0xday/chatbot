using System;

namespace Core
{
    public partial class Core
    {
        private const int IO_PLUGIN_INIT_TIMEOUT_SEC = 10;

        public string Init(string plugin)
        {
            dynamic pluginObject;

            var pluginLoader = new PluginLoader();

            // fixme
            try
            {
                pluginObject = pluginLoader.LoadPlugin(plugin);
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
    }
}
