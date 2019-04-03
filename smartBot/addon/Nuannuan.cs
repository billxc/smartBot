using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace smartBot.Addon
{
    class Nuannuan : SimpleCommandAddOn
    {
        public static readonly Nuannuan Instance = new Nuannuan();

        private readonly string _url = "http://yotsuyu.yorushika.tk:5000/";
        private static readonly HttpClient client = new HttpClient();
        private static Regex rx = new Regex(@"^/?nuannuan(\s.*)?$",RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override string Info()
        {
            return "nuannuan:查看本周金蝶暖暖作业";
        }

        public override string onMessage(string input)
        {
            input = input.TrimStart();
            if (!rx.IsMatch(input))
            {
                return null;
            }
            var html = client.GetStringAsync(_url).Result;
            var body = JsonConvert.DeserializeObject<Dictionary<string, string>>(html);
            if (body.TryGetValue("success", out string success)
                && success == "true"
                && body.TryGetValue("content", out string content))
            {
                return content + "\nPowered by 露儿[Yorushika]";
            }
            else
            {
                return "Error";
            }
          
        }
    }
}
