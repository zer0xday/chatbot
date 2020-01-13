using System;
using System.Collections.Generic;
using System.Text;

namespace PolfanConnector
{
    public class Plugin
    {
        public bool IsConnected()
        {
            return true;
        }

        public void SendMessage(string messageContent)
        {

        }

        public Tuple<string, string> GetMessage()
        {
            Tuple<string, string> message = new Tuple<string, string>("test", "test");

            return message;
        }

        public void Init(string botName)
        {

        }
    }
}
