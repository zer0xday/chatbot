using System;
using System.IO;
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
                CreateConsole();
            }

            Console.WriteLine($"Rozmowa z botem {botName}");
            Console.WriteLine("Podaj swój pseudonim:");
            this.userName = Console.ReadLine();
            this.botName = botName;
            this.isReady = true;
        }

        public void SendMessage(string text)
        {
            Console.WriteLine($"{this.botName}: {text}");
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

        async private void getAsyncInput()
        {
            await Task.Run(() =>
            {
                this.messagesFromUser.Enqueue(Console.ReadLine());
            });

            getAsyncInput();
        }

        private static void CreateConsole()
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
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("kernel32")]
        static extern bool AllocConsole();
    }
}
