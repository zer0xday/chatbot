using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatBot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Tests
{
    [TestClass()]
    public class CoreTests
    {
        [TestMethod()]
        public void SendMessageTest()
        {
            var core = new Core();
            var message = "test string";

            Assert.ThrowsException<Core.PluginNotReadyException>(() =>
            {
                core.SendMessage(message);
            });
        }

        [TestMethod()]
        public void DisconnectTest()
        {
            var core = new Core();

            Assert.ThrowsException<Core.PluginNotReadyException>(() =>
            {
                core.Disconnect();
            });
        }
    }
}