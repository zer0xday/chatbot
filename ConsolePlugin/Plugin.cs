using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsolePlugin
{
    public class Plugin
    {
        public bool IsReady
        {
            set { }
            get { return this.isReady; }
        }

        private bool isReady = false;
        private string botName;
        private string userName;
        private Queue<string> messagesFromUser = new Queue<string>();

        public void Init(string botName)
        {
            if (GetConsoleWindow() == IntPtr.Zero)
            {
                AllocConsole();
            }

            Console.WriteLine($"[=== Rozmowa z botem {botName} ===]");
            Console.WriteLine("Podaj swój pseudonim:");

            this.userName = Console.ReadLine();
            this.botName = botName;
            this.isReady = true;

            Console.WriteLine( "[=== Rozmowa rozpoczęta. Przywitaj się. ===]");

            getAsyncInput();
        }

        public void End()
        {
            FreeConsole();
        }

        public void SendMessage(string text)
        {
            writeMessage(this.botName, text);
        }

        public Tuple<string, string> GetMessage()
        {
            var message = "";

            try
            {
                message = this.messagesFromUser.Dequeue();
            } catch (Exception)
            {
                return null;
            }

            return new Tuple<string, string>(this.userName, message);
        }

        private void writeMessage(string nick, string message)
        {
            Console.WriteLine($"{nick}: {message}");
        }

        async private void getAsyncInput()
        {
            await Task.Run(() =>
            {
                var message = Console.ReadLine();

                if (message.Length > 0)
                {
                    // overwrite last line in console
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    writeMessage(this.userName, message);
                    this.messagesFromUser.Enqueue(message);
                }
            });

            getAsyncInput();
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();
    }
}
