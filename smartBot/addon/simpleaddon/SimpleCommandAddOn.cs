using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using smartBot.manager;
using smartBot.model;

namespace smartBot.manager
{
    abstract class SimpleCommandAddOn : IBotAddOn
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public abstract string onMessage(string input);
      
        public void Action(Bot bot, CoolqEvent message)
        {
            Logger.Debug($"{bot.QQ} receives {message.post_type}");
            if (message is CoolqMessage coolqMessage)
            {
                Logger.Debug($"{bot.QQ} receives message from {coolqMessage.user_id}");
                var reply = onMessage(coolqMessage.message);
                if (string.IsNullOrEmpty(reply))
                {
                    return;
                }
                switch (coolqMessage.message_type)
                {
                    case "group":
                        bot.SendGroupMsg(coolqMessage.group_id, reply, false);
                        break;
                    case "private":
                        bot.SendPrivateMsg(coolqMessage.user_id, reply, false);
                        break;
                    case "discuss":
                        bot.SendPrivateMsg(coolqMessage.discuss_id, reply, false);
                        break;
                }
            }
        }

        public abstract string Info();
    }
}
