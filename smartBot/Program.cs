using System;
using Fleck;
using NLog;
using smartBot.manager;

namespace smartBot
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.LoadConfiguration("Nlog.config");
            var server = new WebSocketServer("ws://0.0.0.0:8181");
            var bot = new BotManager();
            server.RestartAfterListenError = true;
            server.Start(socket =>
            {
                socket.OnOpen = () => bot.Register(socket);
                socket.OnClose = () => bot.UnRegister(socket);
                socket.OnMessage = message => bot.Action(socket,message);
            });
            while (true)
            {
                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
