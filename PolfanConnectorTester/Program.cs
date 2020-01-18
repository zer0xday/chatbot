using System;
using PolfanConnector;

namespace PolfanConnectorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var plugin = new Plugin();

            plugin.Init("bocik");

            while (plugin.IsConnected())
            {
                var message = plugin.GetMessage();

                if (message == null)
                {
                    continue;
                }

                Console.WriteLine("WIADOMOSC:" + message);

            }

        }
    }
}
