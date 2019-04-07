using NLog;
using smartBot.manager;
using smartBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace smartBot.manager
{
    public class Dps : SimpleCommandAddOn
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<int, Regex> RxMap = new Dictionary<int, Regex>
        {
            { 100 , new Regex(@"series.data.push\([+-]?(0|([1-9]\d*)\.\d+)?\)")},
            { 99 , new Regex(@"series99.data.push\([+-]?(0|([1-9]\d*)\.\d+)?\)")},
            { 95 , new Regex(@"series95.data.push\([+-]?(0|([1-9]\d*)\.\d+)?\)")},
            { 75 , new Regex(@"series75.data.push\([+-]?(0|([1-9]\d*)\.\d+)?\)")},
            { 50 , new Regex(@"series50.data.push\([+-]?(0|([1-9]\d*)\.\d+)?\)")},
            { 25 , new Regex(@"series25.data.push\([+-]?(0|([1-9]\d*)\.\d+)?\)")},
            { 10 , new Regex(@"series10.data.push\([+-]?(0|([1-9]\d*)\.\d+)?\)")},
        };

        private static readonly Regex _rx = new Regex(@"/?dps\s+(\S+)\s+(\S+)(\s+\S+)?", RegexOptions.Compiled);

        public override string Info()
        {
            return "dps:查询国际服同期DPS排名，例：/dps 8s 骑士";
        }

        public override string onMessage(string input)
        {
            if (!_rx.IsMatch(input))
            {
                return null;
            }
            var groups = _rx.Match(input).Groups;
            if (groups.Count != 4)
            {
                Logger.Error($"I cannot be, str is {input}");
                return null;
            }


            var boss = groups[1].ToString();
            var ff14ClassAlias = groups[2].ToString();
            var dpsStr = groups[3].ToString();
            if (string.IsNullOrEmpty(dpsStr))
            {
                GetQuestAndBossId(boss, out int questId, out int bossId, out int day);
                var dpsList = CrawlDpsListAsync(questId, bossId, ff14ClassAlias, day).Result;
                return $"{boss} {ff14ClassAlias} day#{day}:\n" +
                    $"10% : {dpsList[0]}\n" +
                    $"25% : {dpsList[1]}\n" +
                    $"50% : {dpsList[2]}\n" +
                    $"75% : {dpsList[3]}\n" +
                    $"95% : {dpsList[4]}\n" +
                    $"99% : {dpsList[5]}\n" +
                    $"100% : {dpsList[6]}\n";
            }
            return !double.TryParse(dpsStr, out double dps) ? "DPS格式不正确" : "Un supported";
        }

        public void GetQuestAndBossId(string bossNameOrAlias, out int questId, out int bossId, out int day)
        {
            //(1043, 'Suzaku', '极朱雀', '{"nickname":["南条爱乃","征婚战","朱雀"]}', 1537920000, 1537920000, 15, 0, 0);
            //(60, 'Chaos', '卡奥斯', '{"nickname":["卡卡","o9s","9s","O9S","9S"]}', 1537660800, 1537833600, 25, 46, 0),
            if (bossNameOrAlias == "极朱雀" || bossNameOrAlias == "朱雀")
            {
                questId = 15;
                bossId = 1043;
                day = -1;
            }
            else if (bossNameOrAlias == "卡奥斯" || bossNameOrAlias == "o9s")
            {
                questId = 25;
                bossId = 60;
                day = -1;
            }
            else
            {
                throw new Exception();
            }

        }


        public async Task<List<double>> CrawlDpsListAsync(int questId, int bossId, string jobName, int day)
        {
            var fflogs_url = $"https://www.fflogs.com/zone/statistics/table/{questId}/dps/{bossId}/100/8/1/100/1000/7/0/Global/{jobName}/All/0/normalized/single/0/-1/";
            Logger.Debug("url is " + fflogs_url);
            var str = await client.GetStringAsync(fflogs_url);
            var percentage_list = new List<int> { 10, 25, 50, 75, 95, 99, 100 };

            return percentage_list.Select(perc =>
            {
                var findResult = RxMap[perc].Matches(str);
                Logger.Debug(findResult.Count);
                Logger.Debug(day);
                var dps = double.Parse(findResult[findResult.Count + day].Groups[1].ToString());
                return dps;
            }).ToList();
        }
    }
}
