using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NLog;
using smartBot.manager;
using smartBot.model;

namespace smartBot.manager.simpleaddon
{
    class Help : IBotAddOn
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static readonly Help Instance = new Help();
        private static Regex rx = new Regex(@"^/?help(\s.*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public void Action(Bot bot, CoolqEvent message)
        {
            Logger.Debug($"{bot.QQ} receives {message.post_type}");
            if (message is CoolqMessage coolqMessage)
            {
                Logger.Debug($"{bot.QQ} receives message from {coolqMessage.user_id}");
                if (!rx.IsMatch(coolqMessage.message))
                {
                    return;
                }
                var reply = string.Join("\n", bot.AddOnList
                    .Select(addon => addon.Info())
                    .Where(s => !string.IsNullOrEmpty(s)));
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

        public string Info()
        {
            return "help: 展示本条信息";
        }
    }
}
