using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace ConsolePlugin
{
    public class Plugin
    {
        private const string EXIT_COMMAND = "!exit";

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
                openConsole();                
            }

            Console.Title = $"Rozmowa z botem {botName}";
            Console.WriteLine("Plugin konsoli służy wyłączonie do lokalnych testów bota.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Aby zakończyć, wpisz \"{EXIT_COMMAND}\". Zamknięcie okna konsoli, spowoduje zamknięcie programu.\n");
            Console.ResetColor();
            Console.WriteLine("[========== Podaj swój pseudonim. ==========]");

            this.userName = Console.ReadLine();
            this.botName = botName;
            this.isReady = true;

            Console.WriteLine( "\n[=== Rozmowa rozpoczęta - przywitaj się. ===]");

            getAsyncInput();
        }

        public void End()
        {
            isReady = false;
            closeConsole();
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
            bool repeat = true;

            await Task.Run(() =>
            {
                var message = "";

                try
                {
                    message = Console.ReadLine();
                } 
                catch (System.IO.IOException)
                {
                    repeat = false;
                    return;
                }

                if (message != null && message.Length > 0)
                {
                    if (message == EXIT_COMMAND)
                    {
                        End();
                        repeat = false;
                        return;
                    }

                    // overwrite last line in console
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    writeMessage(this.userName, message);
                    this.messagesFromUser.Enqueue(message);
                }
            });

            if (repeat)
            {
                getAsyncInput();
            }
        }

        private void openConsole()
        {
            AllocConsole();
            
            TextWriter writer = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
            Console.SetOut(writer);
            Console.OutputEncoding = Encoding.UTF8;

            StreamReader reader = new StreamReader(Console.OpenStandardInput());
            Console.SetIn(reader);
            Console.InputEncoding = Encoding.UTF8;
        }

        private void closeConsole()
        {
            FreeConsole();
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
