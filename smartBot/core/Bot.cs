using System;
using System.Collections.Generic;
using System.Text;
using Fleck;

namespace smartBot
{
    class Bot
    {
        public void OnMessage(IWebSocketConnection socket,string message)
        {
            Console.WriteLine(message);
//            socket.Send(message);
        }
    }
}
