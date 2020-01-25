using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatBot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Tests
{
    [TestClass()]
    public class CoreTests
    {
        public string PLUGIN_PATH = @"C:\Users\Szado\source\repos\cs-college-app\plugins\PolfanConnector.dll";

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

        [TestMethod()]
        [Timeout(10000)]
        public void OnStateChangeTest()
        {
            var core = new Core();
            var stop = false;
            core.OnStateChange = (bool isReady) =>
            {
                stop = true;
                Assert.IsTrue(isReady);
            };

            core.Connect("Bocik12321", PLUGIN_PATH);
            
            while (!stop) { }
        }

        [TestMethod()]
        public void OnMessageTest()
        {
            var core = new Core();
            var stop = false;
            core.OnSystemMessage = (string message) =>
            {
                stop = true;
                Assert.IsTrue(message.Length > 0);
            };

            core.Connect("Bocik32123", PLUGIN_PATH);

            while (!stop) { }
        }
    }
}