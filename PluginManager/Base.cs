using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager
{
    public class Base
    {
        public void SendMessage(string messageContent, string conversationId, string[] receivers)
        {
            
        } 

        public Tuple <string, string, string> GetMessage()
        {
            var message = new Tuple<string, string, string>(
                    "", "", ""
                );
            return message;
        }
    }
}
