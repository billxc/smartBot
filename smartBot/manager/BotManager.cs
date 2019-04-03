using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Fleck;
using Newtonsoft.Json;
using NLog;
using smartBot.manager.simpleaddon;
using smartBot.model;

namespace smartBot.manager
{
    class BotManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static readonly ConcurrentDictionary<long, Bot> BotMap = new ConcurrentDictionary<long, Bot>();

        public void Action(IWebSocketConnection socket,string eventString)
        {
            var qq = GetQQ(socket);
            if (qq.HasValue)
            {
                var bot = BotMap[qq.Value];
                bot.Action(eventString);
            }
        }

        public void Register(IWebSocketConnection socket)
        {
            var qq = GetQQ(socket);
            if (qq.HasValue)
            {
                Logger.Info($"Register qq {qq}");
                BotMap[qq.Value] = new Bot(qq.Value, socket);
                LoadAddOns(BotMap[qq.Value]);
                //TODO init qq, load config
            }
        }

        private static void LoadAddOns(Bot bot)
        {
            Load(bot, Help.Instance);
            Load(bot, Nuannuan.Instance);
            Load(bot, new Test());
            Load(bot, new Weather());
        }

        private static void Load(Bot bot, IBotAddOn addon)
        {
            bot.AddOnList.Add(addon);
            Logger.Debug($"{bot.QQ} load {addon.GetType()}");
        }



        public void UnRegister(IWebSocketConnection socket)
        {
            var qq = GetQQ(socket);
            if (qq.HasValue)
            {
                Logger.Info($"UnRegister qq {qq}");
                BotMap.TryRemove(qq.Value, out Bot v);
            }
           
        }

        static long? GetQQ(IWebSocketConnection socket)
        {
            var connectionInfo = socket.ConnectionInfo;
            if (!connectionInfo.Headers.ContainsKey("X-Self-ID"))
            {
                return null;
            }
            var qqStr = connectionInfo.Headers["X-Self-ID"];
            var success = long.TryParse(qqStr, out long qq);
            return success ? qq : (long?)null;
        }

    }
}
