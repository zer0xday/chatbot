using System;
using System.Threading.Tasks;
using PolfanConnector;
using System.IO;
using System.Runtime.InteropServices;

namespace PolfanConnectorTester
{
    class Program
    {
        private static Plugin plugin;

        static void Main(string[] args)
        {
            plugin = new Plugin();
            plugin.Init("bocik");

            //CreateConsole();

            Console.WriteLine("Inicjowanie pluginu...");

            while (!plugin.IsReady) // tu trzeba ogarnąć timeout'a
            {
            }

            Console.WriteLine("Plugin załadowany!");

            getAsyncInput();

            while (plugin.IsReady)
            {
                var message = plugin.GetMessage();

                if (message == null)
                {
                    continue;
                }

                Console.WriteLine($"Wiadomość do przetworzenia od {message.Item1}: {message.Item2}");
            }

            Console.WriteLine("Plugin przerwał działanie");
        }

        async private static void getAsyncInput()
        {
            await Task.Run(() =>
            {
                string text = Console.ReadLine();
                plugin.SendMessage(text);
            });
            AllocConsole();

            getAsyncInput();
        }

        public static void CreateConsole()
        {
            AllocConsole();

            // stdout's handle seems to always be equal to 7
            IntPtr defaultStdout = new IntPtr(7);
            IntPtr currentStdout = GetStdHandle(StdOutputHandle);

            if (currentStdout != defaultStdout)
                // reset stdout
                SetStdHandle(StdOutputHandle, defaultStdout);

            // reopen stdout
            TextWriter writer = new StreamWriter(Console.OpenStandardOutput())
            { AutoFlush = true };
            Console.SetOut(writer);
        }

        // P/Invoke required:
        private const UInt32 StdOutputHandle = 0xFFFFFFF5;
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(UInt32 nStdHandle);
        [DllImport("kernel32.dll")]
        private static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);
        [DllImport("kernel32")]
        static extern bool AllocConsole();
    }
}
