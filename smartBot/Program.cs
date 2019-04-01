using System;
using Fleck;

namespace smartBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new WebSocketServer("ws://0.0.0.0:8181");
            var bot = new Bot();
            server.RestartAfterListenError = true;
            server.Start(socket =>
            {
                socket.OnOpen = () => Console.WriteLine("Open!");
                socket.OnClose = () => Console.WriteLine("Close!");
                socket.OnMessage = message => bot.OnMessage(socket,message);
            });
            while (true)
            {
                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
