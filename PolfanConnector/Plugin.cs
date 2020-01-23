using System;
using System.Text.RegularExpressions;

namespace PolfanConnector
{
    public class Plugin
    {
        private const string CHAT_SERVER_URL = "ws://s1.polfan.pl:14080";
        private const string CHAT_ROOM_NAME = "gabinet";
        private const string CHAT_MESSAGE_PATTERN = "<font color=.{7}><b>(.*)</b></font>: (.*)"; // <font color=#ff0000><b>NICK</b></font>: MESSAGE

        public bool IsReady
        {
            set { }
            get { return connection.IsLoggedIn; }
        }

        private Connection connection = new Connection();

        public void SendMessage(string messageContent)
        {
            messageContent = Regex.Replace(messageContent, @"\s+", "");

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

            if (message.GetInt(0) == 610) // ordinary chat message
            {
                var tuple = convertMessageIntoTuple(message);
                var realBotNick = "~" + this.connection.Settings.Nick;

                // return null, if it's bot message
                return tuple != null && tuple.Item1 == realBotNick ? null : tuple;
            }

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

        public void End()
        {
            this.connection.Disconnect();
        }

        private Tuple<string, string> convertMessageIntoTuple(Message message)
        {
            var regex = new Regex(CHAT_MESSAGE_PATTERN);
            Match match = regex.Match(message.GetString(0));
            var tuple = new Tuple<string, string>(match.Groups[1].Value, match.Groups[2].Value);

            // some messages can be a system warnings - they have no nickname
            // we are not interested in them
            return tuple.Item1.Length > 0 && tuple.Item2.Length > 0 ? tuple : null;
        }
    }
}
