using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager
{
    public class Base
    {
        private string GetAnswer(string messageContent)
        {
            string answer = "";

            switch(messageContent)
            {
                default: break;
            }

            return answer;
        }

        public void SendMessage(string messageContent, string conversationId, string[] receivers)
        {
            string message = GetAnswer(messageContent);

        } 

        public Tuple <string, string, string> GetMessage()
        {
            var message = new Tuple<string, string, string>
                (
                    "", "", ""
                );
            return message;
        }

        public void setBotName(string name)
        {

        }
    }
}
