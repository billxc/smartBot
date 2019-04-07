using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace smartBot.Service
{
    public class DpsService
    {
        private static readonly HttpClient client = new HttpClient();
        public static readonly DpsService Instance = new DpsService();

        public static Dictionary<string,int> BossAliasMap = new Dictionary<string, int>(); 
        public static Dictionary<string, FFXIVClass> ClassAliasMap = new Dictionary<string, FFXIVClass>();

        public static List<FFXIVClass> ClassList= new List<FFXIVClass>();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public DpsService()
        {
            LoadLocally();
            LoadClassRemotely();
        }

        public int GetBossIdByAlias(string aliasOrId)
        {
            if (BossAliasMap.TryGetValue(aliasOrId,out int id))
            {
                return id;
            }
            return -1;
        }

        public int GetClassIdByAlias(string aliasOrId)
        {
            if (ClassAliasMap.TryGetValue(aliasOrId, out FFXIVClass clz))
            {
                return clz.id;
            }
            return -1;
        }

        public void LoadLocally()
        {
            //load class
            var newClassAliasMap = new Dictionary<string, FFXIVClass>();
            var lines = System.IO.File.ReadAllLines(@"data\class.csv");
            Console.WriteLine(lines.Count());
            var classes = lines.Skip(1).Select(line => new FFXIVClass(line));
            foreach (var c in classes)
            {
                foreach(var a in c.alias)
                {
                    Console.WriteLine($"add {a} {c.id}");
                    newClassAliasMap.Add(a,c);
                }
                Console.WriteLine($"add {c.name_cn} {c.id}");
                newClassAliasMap.Add(c.name_cn, c);
            }

            ClassAliasMap = newClassAliasMap;

            //load boss
            var bossLines = System.IO.File.ReadAllLines(@"data\boss.csv");
            LoadBoss(bossLines);

        }

        public void LoadBoss(string[] lines)
        {
        }

        public async Task LoadClassRemotely()
        {
            var newClassAliasMap = new Dictionary<string, FFXIVClass>();
            //TODO make this url configurable
            var content = await client.GetStringAsync("https://raw.githubusercontent.com/billxc/smartBot/master/smartBot/data/class.csv");
            Logger.Debug(content);
            var lines = content.Split("\n");
            Logger.Debug(lines.Length);
            var classes = lines.Skip(1).Select(line => new FFXIVClass(line));
            foreach (var c in classes)
            {
                foreach (var a in c.alias)
                {
                    Logger.Debug($"LoadRemotely add {a} {c.id}");
                    newClassAliasMap.Add(a, c);
                }
                Logger.Debug($"LoadRemotely add {c.name_cn} {c.id}");
                newClassAliasMap.Add(c.name_cn, c);
            }

            ClassAliasMap = newClassAliasMap;
        }

        //TODO check line data format
        public class FFXIVClass
        {
            public int id;
            public string name;
            public string name_cn;
            public List<string> alias;

            public FFXIVClass(string lineData)
            {
                var tokens = lineData.Split(',');
                id = int.Parse(tokens[0]);
                name = tokens[1].TrimStart().TrimEnd();
                name_cn = tokens[2].TrimStart().TrimEnd();
                var aliasStr = tokens[3].TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(aliasStr))
                {
                    alias = new List<string>(aliasStr.Split("/"));
                    Console.WriteLine(string.Join(",", alias));
                }
                else
                {
                    alias = new List<string>();
                }
            }
        }

        public class FFXIVBoss
        {
            //id,name,name_cn,nickname,add_time,cn_add_time,quest_id,parsed_days
            public int id;
            public string name;
            public string name_cn;
            public List<string> alias;
            public DateTimeOffset add_time;
            public DateTimeOffset cn_add_time;
            public int quest_id;
            public int parsed_days;

            public FFXIVBoss(string lineData)
            {
                var tokens = lineData.Split(',');
                id = int.Parse(tokens[0]);
                name = tokens[1].TrimStart().TrimEnd();
                name_cn = tokens[2].TrimStart().TrimEnd();
                var aliasStr = tokens[3].TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(aliasStr))
                {
                    alias = new List<string>(aliasStr.Split("/"));
                    Console.WriteLine(string.Join(",", alias));
                }
                else
                {
                    alias = new List<string>();
                }

                add_time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(tokens[4]));
                cn_add_time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(tokens[5]));
                quest_id = int.Parse(tokens[6]);
                parsed_days = int.Parse(tokens[7]);
            }

        }
    }
}
