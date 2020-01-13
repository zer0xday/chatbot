using System;
using PolfanConnector;
using System.Threading.Tasks;

namespace PolfanIOPluginTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new ConnectionSettings()
            {
                UriString = "ws://s1.polfan.pl:14080",
                Nick = "aaaaa"
            };

            var connection = new Connection();



            connection.Connect(settings);

            Console.WriteLine("Logujemy");

            var message = new Message();
            message
                .AddInt(1400)
                .AddString("aaaaa", "aaaaa", "gabinet", "", "", "", "c#ioplugin");

            connection.SendMessage(message);

            wri(connection).Wait();


        }

        static async Task wri(Connection connection)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    var message = connection.ReceiveMessage();

                    if (message != null)
                    {
                        Console.WriteLine(message);
                    }



                    
                }
            });
        }
    }
}
