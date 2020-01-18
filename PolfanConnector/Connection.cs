using System;
using System.Collections.Generic;
using System.IO;
using Websocket.Client;


namespace PolfanConnector
{
    public class Connection
    {
        protected WebsocketClient wsClient;
        protected Queue<Message> receivedMessages = new Queue<Message>();
        protected ConnectionSettings settings;

        public void Connect(ConnectionSettings settings)
        {
            this.settings = settings;

            wsClient = new WebsocketClient(settings.Uri);
            wsClient.ReconnectionHappened.Subscribe(type => handleReconnect(type.Type));
            wsClient.MessageReceived.Subscribe(msg => handleIncomingFrame(msg.ToString()));
            wsClient.Start();
        }

        public Message ReceiveMessage()
        {
            try
            {
                return receivedMessages.Dequeue();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SendMessage(Message message)
        {
            Console.WriteLine($"SendMessage: {message.GetJson()}");
            wsClient.Send(message.GetJson());
        }

        protected void handleIncomingFrame(string frame)
        {
            Console.WriteLine($"handleIncomingFrame: {frame}");

            var message = new Message();
            message.SetJson(frame);

            try
            {
                if (message.GetInt(0) == 1)
                {
                    sendPong();
                    return;
                }
            } 
            catch (IndexOutOfRangeException)
            { }

            receivedMessages.Enqueue(message);
        }

        protected void handleReconnect(ReconnectionType type)
        {
            Console.WriteLine($"handleReconnect: {type}, {settings.UriString}, {settings.Nick}, {settings.Room}");
            sendLogIn();
        }

        protected void sendLogIn()
        {
            var message = new Message();
            message
                .AddInt(1400)
                .AddString(settings.Nick)
                .AddString(settings.Password)
                .AddString("")
                .AddString(settings.Room)
                .AddString("")
                .AddString("")
                .AddString("")
                .AddString("BocikC#");

            SendMessage(message);
        }

        protected void sendPong()
        {
            var message = new Message();
            message.AddInt(2);

            SendMessage(message);
        }
    }
}
