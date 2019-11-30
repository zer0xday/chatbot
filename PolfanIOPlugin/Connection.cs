using System;
using System.Collections.Generic;
using System.IO;
using Websocket.Client;

namespace PolfanIOPlugin
{
    public class Connection
    {
        protected WebsocketClient wsClient;
        protected Queue<Message> receivedMessages = new Queue<Message>();

        public void Connect(ConnectionSettings settings)
        {
            wsClient = new WebsocketClient(settings.Uri);
            wsClient.ReconnectionHappened.Subscribe(type => Console.WriteLine($"Reconnect: {type}"));
            wsClient.MessageReceived.Subscribe(msg => EnqueueFrame(msg.ToString()));
            wsClient.Start();
        }

        public Message ReceiveMessage()
        {
            try
            {
                return receivedMessages.Dequeue();
            } catch (Exception)
            {
                return null;
            }
        }

        public void SendMessage(Message message)
        {
            wsClient.Send(message.GetJson());
        }

        protected void EnqueueFrame(string frame)
        {
            var message = new Message();
            message.SetJson(frame);
            receivedMessages.Enqueue(message);
        }
    }
}
