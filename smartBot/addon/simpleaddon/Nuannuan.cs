using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using smartBot.Properties;
using NLog;

namespace smartBot.manager
{
    class Nuannuan : SimpleCommandAddOn
    {
        public static readonly Nuannuan Instance = new Nuannuan();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly string _url = "http://yotsuyu.yorushika.tk:5000/";
        private static readonly HttpClient client = new HttpClient();
        private static readonly Regex _rx = new Regex(@"^/?nuannuan(\s.*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public override string Info()
        {
            return "nuannuan:查看本周金蝶暖暖作业";
        }

        public override string onMessage(string input)
        {
            input = input.TrimStart();
            if (!_rx.IsMatch(input))
            {
                return null;
            }

            string content;
            if (_cache.TryGetValue<string>("nuannuan", out content))
            {
                Logger.Debug("get nuannuan via cache");
                return content + "\nPowered by 露儿[Yorushika]";
            }
            else
            {
                content = GetContent();
                if (content != null) {
                    _cache.Set("nuannuan", content,TimeSpan.FromMinutes(1));
                    return content + "\nPowered by 露儿[Yorushika]";
                }
                else
                {
                    return "Error";
                }
            }
        }

        public string GetContent()
        {
            Logger.Debug("get nuannuan online");
            var html = client.GetStringAsync(_url).Result;
            var body = JsonConvert.DeserializeObject<Dictionary<string, string>>(html);
            if (body.TryGetValue("success", out string success)
                && success == "true"
                && body.TryGetValue("content", out string content))
            {
                return content;
            }
            else
            {
                return null;
            }
        }
    }
}
