using System;
using System.Collections.Generic;
using System.Text;

namespace PolfanConnector
{
    public class Plugin
    {
        private const string CHAT_SERVER_URL = "ws://s1.polfan.pl:14080";
        private const string CHAT_ROOM_NAME = "gabinet";

        private Connection connection = new Connection();

        public Plugin()
        {

        }

        public bool IsConnected()
        {
            return true;
        }

        public void SendMessage(string messageContent)
        {
            var message = new Message();
            message
                .AddString(messageContent)
                .AddString(CHAT_ROOM_NAME)
                .AddInt(410);

            this.connection.SendMessage(message);
        }

        public Tuple<string, string> GetMessage()
        {
            var message = this.connection.ReceiveMessage();

            if (message == null) 
            {
                return null;
            }

            try
            {
                if (message.GetInt(0) == 610)
                {
                    return new Tuple<string, string>(message.GetString(0), message.GetString(1));
                }
            } 
            catch (IndexOutOfRangeException)
            {}

            return null;
        }

        public void Init(string botName)
        {
            var connectionSettings = new ConnectionSettings(){
                UriString = CHAT_SERVER_URL,
                Nick = botName,
                Room = CHAT_ROOM_NAME
            };

            this.connection.Connect(connectionSettings);
        }
    }
}
