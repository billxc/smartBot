using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smartBot.Service
{
    public class DpsService
    {
        public static readonly DpsService Instance = new DpsService();
        public static Dictionary<string,int> BossAliasMap; 
        public static Dictionary<string,int> ClassAliasMap; 

        public DpsService()
        {
            LoadLocally();
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
            if (ClassAliasMap.TryGetValue(aliasOrId, out int id))
            {
                return id;
            }
            return -1;
        }

        public void LoadLocally()
        {
            //load class
            var newClassAliasMap = new Dictionary<string, int>();
            var lines = System.IO.File.ReadAllLines(@"C:\code\smartBot\smartBot\bin\Debug\netcoreapp2.1\data\class.csv");
            Console.WriteLine(lines.Count());
            var classes = lines.Skip(1).Select(line => new FFXIVClass(line));
            foreach (var c in classes)
            {
                foreach(var a in c.alias)
                {
                    Console.WriteLine($"add {a} {c.id}");
                    newClassAliasMap.Add(a,c.id);
                }
                Console.WriteLine($"add {c.name_cn} {c.id}");
                newClassAliasMap.Add(c.name_cn, c.id);
            }

            ClassAliasMap = newClassAliasMap;
        }

        class FFXIVClass
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
    }
}
