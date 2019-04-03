using System;
using System.Collections.Generic;
using System.Text;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using smartBot.model;

namespace smartBot
{
    class Bot
    {
        public void OnMessage(IWebSocketConnection socket,string message)
        {
            var cooqEvent =  JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
            if (!cooqEvent.ContainsKey("post_type"))
            {
                return;
            }
            switch (cooqEvent["post_type"])
            {
                case "message":
                    if (! cooqEvent.ContainsKey("message_type"))
                    {
                        return;
                    }
                    switch (cooqEvent["message_type"])
                    {
                        case "private":
                            var privateMessage = JsonConvert.DeserializeObject<PrivateChatEvent>(message);
                            Console.WriteLine($"receive private from {privateMessage.user_id}," +
                                $"message is {privateMessage.message}");
                            break;
                        case "group":
                            var groupMessage = JsonConvert.DeserializeObject<GroupChatEvent>(message);
                            Console.WriteLine($"receive group msg from {groupMessage.user_id}," +
                                $"message is {groupMessage.message}, group id is {groupMessage.group_id}, " +
                                $"sender nickname is {groupMessage.sender.nickname}");
                            break;
                    }
                break;
            }

//            socket.Send(message);
        }
    }
}
