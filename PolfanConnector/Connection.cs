using System;
using System.Collections.Generic;
using Websocket.Client;


namespace PolfanConnector
{
    public class Connection
    {
        public bool IsLoggedIn
        {
            get { return this.isLoggedIn; }
            set { }
        }
        public ConnectionSettings Settings
        {
            set { }
            get
            {
                return settings;
            }
        }

        protected WebsocketClient wsClient;
        protected Queue<Message> receivedMessages = new Queue<Message>();
        protected ConnectionSettings settings;
        protected bool isLoggedIn = false;

        public void Connect(ConnectionSettings settings)
        {
            this.settings = settings;

            wsClient = new WebsocketClient(settings.Uri);
            wsClient.ReconnectionHappened.Subscribe(type => handleReconnect(type.Type));
            wsClient.MessageReceived.Subscribe(msg => handleIncomingFrame(msg.ToString()));
            wsClient.DisconnectionHappened.Subscribe(ev => handleDisconnect());
            wsClient.Start();
        }

        public void Disconnect()
        {
            this.wsClient.Dispose();
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

            switch (message.GetInt(0))
            {
                case 1: // ping
                    sendPong();
                    break;
                case 626: // loggedIn
                    this.isLoggedIn = true;
                    break;
                default:
                    receivedMessages.Enqueue(message);
                    break;
            }
        }

        protected void handleReconnect(ReconnectionType type)
        {
            Console.WriteLine($"handleReconnect: {type}, {settings.UriString}, {settings.Nick}, {settings.Room}");
            sendLogIn();
        }

        protected void handleDisconnect()
        {
            this.isLoggedIn = false;
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
